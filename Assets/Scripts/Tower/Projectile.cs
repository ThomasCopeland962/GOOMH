using UnityEngine;

public class Projectile : MonoBehaviour
{
    private TowerData _data;
    private Vector3 _shootDirection;
    private float _projectileDuration;

    [Tooltip("0 if sprite faces Right, -90 if sprite faces Up")]
    [SerializeField] private float rotationOffset = -90f;

    void Update()
    {
        if (_projectileDuration <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            _projectileDuration -= Time.deltaTime;
            transform.position += _shootDirection * _data.projectileSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(_data.damage);

                if (_data.slowDown > 0)
                {
                    enemy.ApplySlow(_data.slowDown);
                }

                if (_data.knockback > 0)
                {
                    enemy.ApplyKnockback(_data.knockback);
                }
                if (_data.dot > 0)
                {
                    enemy.ApplyDoT(_data.dot);
                }
            }
            gameObject.SetActive(false);
        }
    }

    public void Shoot(TowerData data, Vector3 shootDirection)
    {
        _data = data;
        _shootDirection = shootDirection;
        _projectileDuration = _data.projectileDuration;
        float angle = Mathf.Atan2(_shootDirection.y, _shootDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);
    }
}