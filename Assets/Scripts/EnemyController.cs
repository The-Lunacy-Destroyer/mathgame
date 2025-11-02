using Mono.Cecil.Cil;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
public class EnemyController : EntityController
{
    [SerializeField] FloatingHealthBar healthBar;

    [SerializeField] float moveSpeed = 5.0f;
    Rigidbody2D _rigidbody;
    Transform target;
    Vector2 moveDirection;

    public float projectileSpeed = 100f;
    public GameObject projectilePrefab;

    public float projectileCooldown = 0.5f;
    private float _launchTimer;
    private bool _canLaunchProjectile = false;

    public float damageScale = 1f;

    private void Start()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        _rigidbody = GetComponent<Rigidbody2D>();
        target = GameObject.Find("Player").transform;
        _launchTimer = projectileCooldown;
    }
    private void Update()
    {
        healthBar.UpdateHealthBar(CurrentHealth, maxHealth);
        if (target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _rigidbody.rotation = angle;
            moveDirection = direction;
        }

        _launchTimer -= Time.deltaTime;
        if (_launchTimer < 0)
        {
            _canLaunchProjectile = true;
            _launchTimer = projectileCooldown;
        }
    }
    private void FixedUpdate()
    {
        if (target)
        {
            _rigidbody.linearVelocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
        }
        LaunchProjectile();
    }

    private void LaunchProjectile()
    {
        if (_canLaunchProjectile)
        {
            GameObject projectileObject = Instantiate(projectilePrefab, _rigidbody.position, Quaternion.identity);
            BulletController projectile = projectileObject.GetComponent<BulletController>();

            projectile.bulletDamage *= damageScale;
            projectile.Launch(moveDirection, projectileSpeed);
            _canLaunchProjectile = false;

        }
    }
}
