using Movement;
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
    
    [field: SerializeField] [field: Range(0f, 1f)] 
    public float Slowdown { get; set; } = 0.9f; 
    [field: SerializeField] public float MoveForce { get; set; } = 8f;
    [field: SerializeField] public float MaxSpeed { get; set; } = 10f;

    public float rotationSpeed = 5f;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _shootingSystem = GetComponent<EntityShootingController>();
        _spaceGunTransform = transform.Find("SpaceGun");
        _mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        if (_shootingSystem && _spaceGunTransform && 
            (Keyboard.current.cKey.isPressed || Mouse.current.leftButton.isPressed))
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

        Vector2 movementVector = GetMovementDirection();
        
        if (movementVector.magnitude > 0)
        {
            movementVector *= 1 + (movementVector - _rigidbody.linearVelocity.normalized).magnitude / 2;
            _rigidbody.AddForce(movementVector * MoveForce);
        }
        else
        {
            _rigidbody.linearVelocity *= Slowdown;
        }
        
        _rigidbody.linearVelocity = Vector2.ClampMagnitude(_rigidbody.linearVelocity, MaxSpeed);
    }

    private Vector2 GetMovementDirection()
    {
        Vector2 movementDirection = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) movementDirection.y++;
        if (Keyboard.current.sKey.isPressed) movementDirection.y--;
        if (Keyboard.current.dKey.isPressed) movementDirection.x++;
        if (Keyboard.current.aKey.isPressed) movementDirection.x--;
        
        return movementDirection.normalized;
    }

    private void RotateGun()
    {

        Vector2 mouseVector = GetPlayerToMouseVector();
        Vector2 direction = mouseVector.normalized;
        Vector2 current = _rigidbody.transform.up.normalized;
        
        _rigidbody.transform.up = Vector2.MoveTowards(current, direction,
            rotationSpeed * (current - direction).magnitude * Time.fixedDeltaTime
        );
    }
    
    private Vector3 GetPlayerToMouseVector()
    {
        if (_mainCamera is null) return Vector3.zero;
        
        Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Mouse.current.position.value);
        return mousePosition - transform.position;
    }
}
