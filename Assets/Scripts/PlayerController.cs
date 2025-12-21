using System;
using System.Collections;
using Health;
using Movement;
using Projectile;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Unity.Mathematics;

public class PlayerController : EntityController, IEntityMovable
{
    private EntityBulletController _bulletSystem;
    private Rigidbody2D _rigidbody;
    private EntityHealthController _healthSystem;
    private Camera _mainCamera;
    
    // Movement
    
    [field: SerializeField] [field: Range(0f, 1f)] 
    public float Slowdown { get; set; } = 0.9f; 
    [field: SerializeField] public float MoveForce { get; set; } = 8f;
    [field: SerializeField] public float MaxSpeed { get; set; } = 10f;

    public float rotationSpeed = 5f;
    
    //Score 
    private float _score;
    public float Score
    {
        get => _score;
        set
        {
            _score = value;
            _scoreText.text = $"Score: {_score}";
        }
    }
    private int _enemyKillCounter;
    public int EnemyKillCounter
    {
        get => _enemyKillCounter;
        set
        {
            _enemyKillCounter = value;
            _enemyKillCounterText.text = $"Enemies killed: {_enemyKillCounter}";
        }
    }
    
    public UIDocument scoreUI;
    private Label _scoreText;
    private Label _enemyKillCounterText;

    private bool _is42;
    private bool _isAbility42;

    private float _oldProjectileCooldown;
    private float _oldProjectileSpeed;
    private float _oldDamageScale;
    private float _oldMaxHealth;
    private float _oldSpreadAngle;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _bulletSystem = GetComponent<EntityBulletController>();
        _healthSystem = GetComponent<EntityHealthController>();
    }

    private void Start()
    {
        _mainCamera = Camera.main;
        _scoreText = scoreUI.rootVisualElement.Q<Label>("ScoreLabel");
        _enemyKillCounterText = scoreUI.rootVisualElement.Q<Label>("EnemyKillLabel");

        _oldProjectileCooldown = _bulletSystem.projectileCooldown;
        _oldProjectileSpeed = _bulletSystem.projectileSpeed;
        _oldDamageScale = _bulletSystem.damageScale;
        _oldMaxHealth = _healthSystem.maxHealth;
        _oldSpreadAngle = _bulletSystem.spreadAngle;
    }

    private void Update()
    {
        if (_is42 && Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            if (!_isAbility42) GetAbility42();
            else RemoveAbility42();
        }
        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            _is42 = true;
            StartCoroutine(nameof(WaitForDigit2));
        }
    }

    private IEnumerator WaitForDigit2()
    {
        yield return new WaitForSeconds(3f);
        _is42 = false;
    }

    private void GetAbility42()
    {
        _isAbility42 = true;
        _bulletSystem.projectileCooldown = 0.001f;
        _bulletSystem.projectileSpeed = 900f;
        _bulletSystem.damageScale = 100f;
        _bulletSystem.spreadAngle = 40f;
            
        if (_healthSystem)
        {
            _healthSystem.maxHealth = 100000;
            _healthSystem.CurrentHealth = 100000;
        }
    }
    private void RemoveAbility42()
    {
        _isAbility42 = false;
        _bulletSystem.projectileCooldown = _oldProjectileCooldown;
        _bulletSystem.projectileSpeed = _oldProjectileSpeed;
        _bulletSystem.damageScale = _oldDamageScale;
        _bulletSystem.spreadAngle = _oldSpreadAngle;
            
        if (_healthSystem)
        {
            _healthSystem.maxHealth = _oldMaxHealth;
            _healthSystem.CurrentHealth = _oldMaxHealth;
        }
    }

    private void FixedUpdate()
    {
        if (_bulletSystem && 
            (Keyboard.current.cKey.isPressed || Mouse.current.leftButton.isPressed))
        {
            _bulletSystem.Shoot(
                transform.position + transform.up * 0.3f, 
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
            rotationSpeed * math.radians(Vector2.Angle(current, direction)) * 0.01f
        );
    }
    
    private Vector3 GetPlayerToMouseVector()
    {
        if (_mainCamera is null) return Vector3.zero;
        
        Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Mouse.current.position.value);
        return mousePosition - transform.position;
    }
}
