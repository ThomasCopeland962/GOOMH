using System;
using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData data;
    [SerializeField] private GameObject slowVisual;
    [SerializeField] private GameObject dotVisual;
    public EnemyData Data => data;

    public static event Action<EnemyData> OnEnemyReachedEnd;
    public static event Action<Enemy> OnEnemyDestroyed;

    private AudioSource _audio;

    private path currentPath;
    private Vector3 _targetPosition;
    private int _currentWaypoint;
    private float _lives;
    private float _maxLives;
    private bool _hasBeenCounted = false;
    private float _speedMultiplier = 1f;
    private Coroutine _slowCoroutine;
    private Coroutine _dotCoroutine;

    private void Awake()
    {
        currentPath = GameObject.Find("Path").GetComponent<path>();
        _audio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _currentWaypoint = 0;
        _targetPosition = currentPath.GetPosition(0);
        _speedMultiplier = 1f; 
        FaceTarget();
    }
    private void OnDisable()
    {
        if (_slowCoroutine != null) StopCoroutine(_slowCoroutine);
        if (_dotCoroutine != null) StopCoroutine(_dotCoroutine);

        if (slowVisual != null) slowVisual.SetActive(false);
        if (dotVisual != null) dotVisual.SetActive(false);

        _slowCoroutine = null;
        _dotCoroutine = null;
    }

    void Update()
    {
        if (_hasBeenCounted) return;

        float currentMoveSpeed = data.speed * _speedMultiplier;
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, currentMoveSpeed * Time.deltaTime);
        
        FaceTarget();

        float relativeDistance = (transform.position - _targetPosition).magnitude;
        if (relativeDistance < .1f)
        {
            if (_currentWaypoint < currentPath.Waypoints.Length - 1)
            {
                _currentWaypoint++;
                _targetPosition = currentPath.GetPosition(_currentWaypoint);
            }
            else
            {
                _hasBeenCounted = true;
                OnEnemyReachedEnd?.Invoke(data);
                gameObject.SetActive(false);
            }
        }
    }
public void ApplyDoT(float dotDamage)
{

    if (_dotCoroutine != null)
        {
            StopCoroutine(_dotCoroutine);
        }
    _dotCoroutine = StartCoroutine(DoTRoutine(dotDamage));
}

private IEnumerator DoTRoutine(float damagePerTick)
{
    float duration = 5f;
    float timer = 0f;
    if (dotVisual != null)dotVisual.SetActive(true);

    while (timer < duration)
    {
        yield return new WaitForSeconds(1f);
        TakeDamage(damagePerTick);
        timer += 1f;
    }
    if (dotVisual != null) dotVisual.SetActive(false);

    _dotCoroutine = null;
}    

    public void ApplySlow(float slowAmount)
    {
        if (_slowCoroutine != null)
        {
            StopCoroutine(_slowCoroutine);
        }

        _slowCoroutine = StartCoroutine(SlowRoutine(slowAmount));
    }

    private IEnumerator SlowRoutine(float slowAmount)
    {
        _speedMultiplier = Mathf.Clamp(1f - slowAmount, 0.1f, 1f);
        if (slowVisual != null) slowVisual.SetActive(true);
        yield return new WaitForSeconds(5f);
        if (slowVisual != null) slowVisual.SetActive(false);
        _speedMultiplier = 1f;
        _slowCoroutine = null;
    }
    public void ApplyKnockback(float knockbackForce)
    {
        if (_currentWaypoint <= 0) return;

        Vector3 backDirection = (currentPath.GetPosition(_currentWaypoint - 1) - transform.position).normalized;
        transform.position += backDirection * knockbackForce;
        float distToPrev = Vector3.Distance(transform.position, currentPath.GetPosition(_currentWaypoint - 1));
            if (distToPrev < 0.2f && _currentWaypoint > 0)
            {
                _currentWaypoint--;
                _targetPosition = currentPath.GetPosition(_currentWaypoint);
            }
    }

    private void FaceTarget()
    {
        Vector3 direction = _targetPosition - transform.position;
            if (direction != Vector3.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
            }
    }


    public void TakeDamage(float damage) {
    if (_hasBeenCounted) return;
    
    _lives -= damage;
    _lives = Math.Max(_lives, 0);

    if (_lives <= 0) {

        _hasBeenCounted = true; 
        
        StartCoroutine(HandleDeath());
    }
}

private IEnumerator HandleDeath() {
    if (_audio != null && data.deathSound != null) {
        _audio.PlayOneShot(data.deathSound);
    }

    if (TryGetComponent<SpriteRenderer>(out var renderer)) renderer.enabled = false;
    if (TryGetComponent<Collider2D>(out var collider)) collider.enabled = false;
    if (slowVisual != null) slowVisual.SetActive(false);
    if (dotVisual != null) dotVisual.SetActive(false);

    OnEnemyDestroyed?.Invoke(this);

    yield return new WaitForSeconds(data.deathSound != null ? data.deathSound.length : 0f);

    StopAllCoroutines(); 
    gameObject.SetActive(false);
}

public void Initialize(float healthMultiplyer) {
    _hasBeenCounted = false;
    _maxLives = data.lives * healthMultiplyer;
    _lives = _maxLives;

    if (TryGetComponent<SpriteRenderer>(out var renderer)) renderer.enabled = true;
    if (TryGetComponent<Collider2D>(out var collider)) collider.enabled = true;
    
    if (slowVisual != null) slowVisual.SetActive(false);
    if (dotVisual != null) dotVisual.SetActive(false);
}


}
