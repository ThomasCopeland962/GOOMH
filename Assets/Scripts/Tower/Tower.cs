using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private TowerData data;
    [SerializeField] private float rotationOffset = -90f; 
    
    private CircleCollider2D _circleCollider;
    private List<Enemy> _enemiesInRange = new List<Enemy>();
    private ObjectPooler _projectilePool;
    private AudioSource _audio;
    private float _shootTimer;

    private void OnEnable() => Enemy.OnEnemyDestroyed += HandleEnemyDestroyed;
    private void OnDisable() => Enemy.OnEnemyDestroyed -= HandleEnemyDestroyed;

    private void Start()
    {
        _circleCollider = GetComponent<CircleCollider2D>();
        _circleCollider.radius = data.range;
        _projectilePool = GetComponent<ObjectPooler>();
        _shootTimer = data.shootInterval;
        _audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _enemiesInRange.RemoveAll(enemy => enemy == null || !enemy.gameObject.activeInHierarchy);

        if (_enemiesInRange.Count > 0)
        {
            RotateTowardsTarget(_enemiesInRange[0].transform);
            _shootTimer -= Time.deltaTime;
            
            if (_shootTimer <= 0)
            {
                _shootTimer = data.shootInterval;
                Shoot();
            }
        }
    }

    private void RotateTowardsTarget(Transform target)
    {
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);
    }

    private void Shoot()
    {
        _audio.Play();
        GameObject projectile = _projectilePool.GetPooledObject();
        projectile.transform.position = transform.position;
        projectile.transform.SetParent(null);
        projectile.SetActive(true);
        Vector2 _shootDirection = (_enemiesInRange[0].transform.position - transform.position).normalized;
        projectile.GetComponent<Projectile>().Shoot(data, _shootDirection);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            _enemiesInRange.Add(collision.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            _enemiesInRange.Remove(collision.GetComponent<Enemy>());
        }
    }

    private void HandleEnemyDestroyed(Enemy enemy) => _enemiesInRange.Remove(enemy);
    private void OnDrawGizmos()
    {
        if (data == null) return;

        Gizmos.color = Color.limeGreen;
        float worldRadius = data.range * transform.lossyScale.x;
        Gizmos.DrawWireSphere(transform.position, worldRadius);
    }
}



