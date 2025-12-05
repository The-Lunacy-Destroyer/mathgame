using Interfaces;
using Projectile;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : EntityController, IEntityMovable
{
    private EntityShootingController _shootingSystem;
    private Transform _spaceGunTransform;
    private Rigidbody2D _rigidbody;
    private Camera _mainCamera;
    
    // Movement
    [field: SerializeField] public float Slowdown { get; set; } = 1f;
    [field: SerializeField] public float Speed { get; set; } = 1f;
    [field: SerializeField] public float MaxSpeed { get; set; } = 10f;

    public float rotationSpeed = 5f;
    public float decelerationForce = 1f;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _shootingSystem = GetComponent<EntityShootingController>();
        _spaceGunTransform = transform.Find("SpaceGun");
        _mainCamera = Camera.main;
    }

    void FixedUpdate()
    {
        if (_shootingSystem && _spaceGunTransform && 
            (Keyboard.current.cKey.isPressed || Mouse.current.rightButton.isPressed))
        {
            _shootingSystem.Shoot(
                _spaceGunTransform.position, 
                transform.up);
        }
        Move();
        RotateGun();
    }

    private void Move()
    {
        if (!_rigidbody) return;
        
        Vector2 movementDirection = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) movementDirection.y++;
        if (Keyboard.current.sKey.isPressed) movementDirection.y--;
        if (Keyboard.current.dKey.isPressed) movementDirection.x++;
        if (Keyboard.current.aKey.isPressed) movementDirection.x--;
        
        if (movementDirection.magnitude > 0)
        {
            float deceleration = Vector2.Angle(_rigidbody.totalForce, movementDirection) 
                / 180f * decelerationForce;
            float forceSpeed = Speed * (1 + deceleration);
            _rigidbody.AddForce(movementDirection * forceSpeed);
        }
        else if (_rigidbody.linearVelocity.magnitude >= Slowdown)
        {
            _rigidbody.linearVelocity -= _rigidbody.linearVelocity.normalized * Slowdown;
        }
        else
        {
            _rigidbody.linearVelocity = Vector2.zero;
        }
        
        _rigidbody.linearVelocity = Vector2.ClampMagnitude(_rigidbody.linearVelocity, MaxSpeed);
    }

    void RotateGun()
    {
        if (!Mouse.current.leftButton.isPressed) return;
        Vector2 mouseVector = GetPlayerToMouseVector();
        Vector2 direction = mouseVector.normalized;
        
        _rigidbody.transform.up = Vector2.MoveTowards(
            _rigidbody.transform.up,
            direction,
            rotationSpeed * Time.deltaTime
        );
    }
    
    Vector3 GetPlayerToMouseVector()
    {
        if (_mainCamera is null) return Vector3.zero;
        
        Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Mouse.current.position.value);
        return mousePosition - transform.position;
    }
}
