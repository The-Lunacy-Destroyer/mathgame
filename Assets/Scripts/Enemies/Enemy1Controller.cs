using Health;
using Movement;
using Projectile;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class Enemy1Controller : EntityController, IEntityMovable, IEntityRadiusStoppable
    {
        private Rigidbody2D _rigidbody;
        private Transform _targetTransform;
        private EntityShootingController _shootingSystem;
        
        // distance between enemy and target at which enemy begins shooting
        public float shootRadius = 8f;
        
        [field: SerializeField] public float MoveForce { get; set; } = 8f;
        [field: SerializeField] public float MaxSpeed { get; set; } = 10f;
        
        [field: SerializeField] public float Slowdown { get; set; } = 0.9f;
        [field: SerializeField] public float StopRadius { get; set; } = 5f;
        
        public float minDeviation = 0.95f;
        public float maxDeviation = 1.25f;
        private float _deviationFactor;

        [Min(1)]
        public int actionCooldown = 1;
        private int _actionCooldownTimer;
        
        private Vector2 _targetVector;
        private Vector2 TargetDirection => _targetVector.normalized;

        public int randomStopRadiusVectorCooldown = 30;
        private int _randomStopRadiusVectorTimer;
        private Vector2 _randomStopRadiusVector;

        public float randomMovementAngle = 45f;
        public float randomMovementCooldown = 30f;
        private float _randomMovementTimer = 0f;
        private Vector2 _movementDirection;
        
        private void Start()
        {
            gameObject.SetActive(false);
            _rigidbody = GetComponent<Rigidbody2D>();
            _shootingSystem = GetComponent<EntityShootingController>();
            _targetTransform = GameObject.Find("Player").transform;
            
            _deviationFactor = Random.Range(minDeviation, maxDeviation);
            _actionCooldownTimer = actionCooldown;
        }
    
        private void FixedUpdate()
        {
            if (_targetTransform)
            {
                _targetVector = _targetTransform.position - transform.position;

                if (_shootingSystem && _targetVector.magnitude <= shootRadius)
                {
                    _shootingSystem.Shoot(_rigidbody.position, TargetDirection);
                }
            
                Move();
            }
        }

        private void Move()
        {
            if (!_rigidbody) return;

            _rigidbody.linearVelocity = Vector2.ClampMagnitude(_rigidbody.linearVelocity, MaxSpeed);
            
            if (_rigidbody.linearVelocity.magnitude > 0 
                && _targetVector.magnitude <= StopRadius)
            {
                if (_actionCooldownTimer <= 0)
                {
                    PerformStopAction();
                    return;
                }
                
                _rigidbody.linearVelocity *= Slowdown;
                
                _actionCooldownTimer--;
            }
            else if (TargetDirection.magnitude > 0)
            {
                float deviation = 1 + (TargetDirection - _rigidbody.linearVelocity.normalized).magnitude * _deviationFactor;
                if (_randomMovementTimer <= 0)
                {
                    _movementDirection = MathUtilities.RotateVector(TargetDirection, 
                        Random.Range(-randomMovementAngle, randomMovementAngle));

                    _randomMovementTimer = randomMovementCooldown;
                }

                _rigidbody.AddForce(_movementDirection * (deviation * MoveForce));
                
                _actionCooldownTimer = actionCooldown;
                _randomMovementTimer--;
            }
        }

        private void PerformStopAction()
        {
            if (_randomStopRadiusVectorTimer <= 0)
            {
                float randomAngle = Random.value < 0.5f ?
                    Random.Range(-50f, -90f) : 
                    Random.Range(50f, 90f);
                
                _randomStopRadiusVector = MathUtilities.RotateVector(TargetDirection, randomAngle);
                _randomStopRadiusVectorTimer = randomStopRadiusVectorCooldown;
            }
            _rigidbody.linearVelocity = _randomStopRadiusVector.normalized * MoveForce;
            _randomStopRadiusVectorTimer--;
        }
    }
}
