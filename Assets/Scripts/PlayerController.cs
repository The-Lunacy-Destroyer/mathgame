using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rigidbody = null;
    private Camera _mainCamera = null;

    public float speed = 1f;
    public float maxSpeed = 10f;
    public float slowdown = 5f;

    public float projectileSpeed = 100f;
    public GameObject projectilePrefab;
    
    public float launchCooldown = 0.2f;
    private float _launchTimer = 0;
    private bool _canLaunchProjectile = true;
    
    public float maxHealth = 100.0f;
    private float _currentHealth = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _launchTimer = launchCooldown;
        _currentHealth = maxHealth;
        _rigidbody = GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        _launchTimer -= Time.deltaTime;
        if (_launchTimer < 0)
        {
            _canLaunchProjectile = true;
            _launchTimer = launchCooldown;
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
        LaunchProjectile();
       
    }
    
    public void ChangePlayerHealth(float amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, maxHealth);
    }

    private Vector3 GetPlayerToMouseVector()
    {
        Vector3 mousepos = _mainCamera.ScreenToWorldPoint(Mouse.current.position.value);
        return mousepos - transform.position;
    }

    private void MovePlayer()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Vector2 direction = GetPlayerToMouseVector().normalized;
            Vector2 movement = direction * (Time.fixedDeltaTime * 100f * speed);
            _rigidbody.AddForce(movement);
            _rigidbody.transform.up = direction;
        }
        else if (_rigidbody.linearVelocity.magnitude > 0)
        {
            _rigidbody.AddForce(-_rigidbody.linearVelocity.normalized * slowdown);
        }
        if (_rigidbody.linearVelocity.magnitude > maxSpeed)
        {
            _rigidbody.linearVelocity = _rigidbody.linearVelocity.normalized * maxSpeed;
        }
    }

    private void LaunchProjectile()
    {
        if (Keyboard.current.cKey.isPressed && _canLaunchProjectile)
        {
            GameObject projectileObject = Instantiate(projectilePrefab, _rigidbody.position, Quaternion.identity);
            BulletController projectile = projectileObject.GetComponent<BulletController>();
            projectile.Launch(_rigidbody.transform.up, projectileSpeed);
            _canLaunchProjectile = false;
        }
    }
}
