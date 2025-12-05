using System;
using Health;
using Interfaces;
using Projectile;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class Enemy1Controller : EntityController, IEntityMovable
    {
        private Transform _targetTransform;
        private EntityShootingController _shootingSystem;
        private EntityHealthController _healthSystem;
        private Rigidbody2D _rigidbody;
    
        public GameObject healthDrop;
    
        public float shootRadius = 3f;
    
        // Movement
        [field: SerializeField] public float Slowdown { get; set; } = 1f;
        [field: SerializeField] public float Speed { get; set; } = 1f;
        [field: SerializeField] public float MaxSpeed { get; set; } = 10f;
    
        public float slowdownRadius = 4f;
        private Vector2 _movementVector;
        private Vector2 MovementDirection => _movementVector.normalized;

        public float minSpeedScale = 1f;
        public float maxSpeedScale = 1f;
        private float _speedScale = 1f;
    
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _shootingSystem = GetComponent<EntityShootingController>();
            _healthSystem = GetComponent<EntityHealthController>();
            _targetTransform = GameObject.Find("Player").transform;
            _movementVector = _targetTransform.position - transform.position;

            minSpeedScale = Math.Min(minSpeedScale, 1);
            RandomizeStats();
        }
    
        void FixedUpdate()
        {
            if (_targetTransform)
            {
                _movementVector = _targetTransform.position - transform.position;

                if (_shootingSystem && _movementVector.magnitude <= shootRadius)
                {
                    _shootingSystem.Shoot(_rigidbody.position, MovementDirection);
                }
            
                Move();
            }
        }

        void OnDestroy()
        {
            if(_healthSystem && _healthSystem.CurrentHealth <= 0)
                Instantiate(healthDrop, transform.position, Quaternion.identity);
        }

        private void Move()
        {
            if (!_rigidbody) return;
        
            float angle = Mathf.Atan2(MovementDirection.x, MovementDirection.y) * Mathf.Rad2Deg;
            _rigidbody.rotation = angle;
        
            float forceAngle = Mathf.Atan2(_rigidbody.totalForce.x, _rigidbody.totalForce.y) * Mathf.Rad2Deg;
        
            if (_movementVector.magnitude > slowdownRadius)
            {
                float forceSpeed = Speed * (1 + Mathf.Abs(angle - forceAngle) / 360f);
                _rigidbody.AddForce(MovementDirection * forceSpeed);
            }
            else if (_rigidbody.linearVelocity.magnitude >= Slowdown)
            {
                _rigidbody.linearVelocity -= _rigidbody.linearVelocity.normalized * Slowdown;
            }
            else
            {
                _rigidbody.linearVelocity = Vector2.zero;
            }

            _rigidbody.linearVelocity = Vector2.ClampMagnitude(
                _rigidbody.linearVelocity, MaxSpeed * _speedScale);
        }

        void RandomizeStats()
        {
            _speedScale = Random.Range(1 - minSpeedScale, 1 + maxSpeedScale);
        }
    }
}
