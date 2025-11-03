using System;
using UnityEngine;

public class EntityController  : MonoBehaviour
{
    private FloatingHealthBar healthBar;

    public float maxHealth = 100.0f;
    private float _currentHealth;

    public float projectileCooldown = 0.5f;
    private float _launchTimer;
    private bool _canLaunchProjectile = true;

    protected Vector2 moveDirection;

    public float projectileSpeed = 100f;
    public GameObject projectilePrefab;

    public float damageScale = 1f;
    protected Rigidbody2D _rigidbody;
    public float CurrentHealth
    {
        get => _currentHealth;
        set => _currentHealth = Mathf.Clamp(value, 0, maxHealth);
    }
    
     void Awake()
    {
        CurrentHealth = maxHealth;
        _launchTimer = projectileCooldown;
        _canLaunchProjectile = false;
        _rigidbody = GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }

    protected virtual void Update()
    {
        _launchTimer -= Time.deltaTime;
        if (_launchTimer < 0)
        {
            _canLaunchProjectile = true;
            _launchTimer = projectileCooldown;
        }
        healthBar.UpdateHealthBar(CurrentHealth, maxHealth);
    }
    
    protected virtual void LaunchProjectile(Vector2 launchDirection)
    {
        if (_canLaunchProjectile)
        {
            GameObject projectileObject = Instantiate(projectilePrefab, _rigidbody.position, Quaternion.identity);
            BulletController projectile = projectileObject.GetComponent<BulletController>();
            
            projectile.Source = this.GetComponent<EntityController>();
            projectile.bulletDamage *= damageScale;
            projectile.Launch(launchDirection, projectileSpeed);
            _canLaunchProjectile = false;

        }
    }
}