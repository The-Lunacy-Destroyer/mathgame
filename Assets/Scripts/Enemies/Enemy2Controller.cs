using Health;
using Movement;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities;

namespace Enemies
{
    public class Enemy2Controller : EntityController, IEntityMovable
    {
        private Rigidbody2D _rigidbody;
        private Transform _targetTransform;
        
        [field: SerializeField] public float MoveForce { get; set; } = 8f;
        [field: SerializeField] public float MaxSpeed { get; set; } = 10f;
        
        [field: SerializeField] public float Slowdown { get; set; } = 0.9f;
        [field: SerializeField] public float StopRadius { get; set; } = 5f;
        public float minStopRadiusScale = 0.75f;
        public float maxStopRadiusScale = 1.25f;
        
        public float minDeviation = 0.95f;
        public float maxDeviation = 1.25f;
        private float _deviationFactor;

        [Min(1)]
        public int actionCooldown = 1;
        private int _actionCooldownTimer;
        
        private Vector2 _targetVector;
        private Vector2 TargetDirection => _targetVector.normalized;

        public int randomStopRadiusVectorCooldown = 100;
        private int _randomStopRadiusVectorTimer;
        private Vector2 _randomStopRadiusVector;

        public float randomMovementAngle = 30f;
        public float randomMovementCooldown = 20f;
        private float _randomMovementTimer = 0f;
        private Vector2 _movementDirection;

        public float stopRadiusMoveForce = 5f;

        public float contactDamage = 5f;
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _targetTransform = GameObject.Find("Player").transform;
            
            _deviationFactor = Random.Range(minDeviation, maxDeviation);
            StopRadius *= Random.Range(minStopRadiusScale, maxStopRadiusScale);
            _actionCooldownTimer = actionCooldown;
        }
    
        private void FixedUpdate()
        {
            if (_targetTransform)
            {
                _targetVector = _targetTransform.position - transform.position;

                Move();
                Rotate();
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
                _randomStopRadiusVectorTimer = 0;
            }
        }

        private void PerformStopAction()
        {
            if (_randomStopRadiusVectorTimer <= 0)
            {
                float randomAngle = Random.Range(-15f, 15f);
                
                _randomStopRadiusVector = MathUtilities.RotateVector(TargetDirection, randomAngle);
                _randomStopRadiusVectorTimer = randomStopRadiusVectorCooldown;
            }
            _rigidbody.linearVelocity = _randomStopRadiusVector.normalized * (MoveForce * stopRadiusMoveForce);
            _randomStopRadiusVectorTimer--;
        }

        private void Rotate()
        {
            transform.up = Vector2.MoveTowards(
                transform.up, TargetDirection, 10 * Time.fixedDeltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            EntityHealthController otherEntityHealth = 
                other.GetComponent<EntityHealthController>();

            if (otherEntityHealth && other.gameObject.CompareTag("Player"))
            {
                otherEntityHealth.CurrentHealth -= contactDamage;
                
                if (otherEntityHealth.CurrentHealth <= 0)
                {
                    Destroy(otherEntityHealth.gameObject);
                }
            }
        }
    }
}