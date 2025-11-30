using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections.Generic;
public class EntityController  : MonoBehaviour
{
    private FloatingHealthBar _healthBar;
    protected Rigidbody2D _rigidbody;
    
    // Entity movement
    public float slowdown = 5f;
    public float speed = 1f;
    public float maxSpeed = 10f;
    
    // Entity health
    public float maxHealth = 100.0f;
    private float _currentHealth;
    public float CurrentHealth
    {
        get => _currentHealth;
        set => _currentHealth = Mathf.Clamp(value, 0, maxHealth);
    }
    
    // Projectiles
    public GameObject projectilePrefab;
    public float projectileSpeed = 100f;
    public float damageScale = 1f;
    public float projectileCooldown = 0.5f;
    public float spreadAngle = 15f;
    private float _launchTimer;
    public  bool _canLaunchProjectile = true;
    
    
     void Awake()
    {
        CurrentHealth = maxHealth;
        _launchTimer = projectileCooldown;
        _canLaunchProjectile = false;
        _rigidbody = GetComponent<Rigidbody2D>();
        _healthBar = GetComponentInChildren<FloatingHealthBar>();
        _healthBar.Source = GetComponent<EntityController>();
    }

    protected virtual void Update()
    {
        AddProjectileLaunchDelay();
        _healthBar.UpdateHealthBar(CurrentHealth, maxHealth);
    }
    
    protected virtual void LaunchProjectile(Vector2 launchPosition, Vector2 launchDirection, bool repeating)
    {
        if (_canLaunchProjectile || repeating)
        {
            GameObject projectileObject = Instantiate(projectilePrefab, launchPosition, Quaternion.identity);
            BulletController projectile = projectileObject.GetComponent<BulletController>();

            projectile.Source = GetComponent<EntityController>();
            projectile.bulletDamage *= damageScale;
            _canLaunchProjectile = false;
            float spread = Random.Range(-spreadAngle, spreadAngle);
            launchDirection = Utilities.RotateVector(launchDirection, spread);
            projectile.Launch(launchDirection, projectileSpeed);

        }
    }



    private void AddProjectileLaunchDelay()
    {
        _launchTimer -= Time.deltaTime;
        if (_launchTimer < 0)
        {
            _canLaunchProjectile = true;
            _launchTimer = projectileCooldown;
        }
    }
}