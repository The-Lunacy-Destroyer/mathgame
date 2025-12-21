using Health;
using Movement;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities;

namespace Enemies
{
    public class Enemy2Controller : BaseEnemy
    {
        public float stopRadius = 5f;
        public float minStopRadiusScale = 0.75f;
        public float maxStopRadiusScale = 1.25f;
        
        public float minDeviation = 0.95f;
        public float maxDeviation = 1.25f;
        private float _deviationFactor;

        [Min(0)]
        public int actionCooldown = 1;
        private int _actionCooldownTimer;

        public int stopRadiusMovementCooldown = 100;
        private int _stopRadiusMovementTimer;
        private Vector2 _randomStopRadiusVector;

        public float stopRadiusMoveForce = 5f;
        public float contactDamage = 5f;
        public float stopRadiusMovementAngle = 15f;
        
        protected override void Awake()
        {
            base.Awake();

            _deviationFactor = Random.Range(minDeviation, maxDeviation);
            stopRadius *= Random.Range(minStopRadiusScale, maxStopRadiusScale);
            _actionCooldownTimer = actionCooldown;
        }
        
        protected override void Start()
        {
            base.Start();
            transform.up = TargetDirection;
        }
    
        private void FixedUpdate()
        {
            if (Target)
            {
                TargetVector = Target.position - transform.position;

                Move();
                Rotate();
            }
        }

        private void Move()
        {
            if (!Rigidbody) return;

            Rigidbody.linearVelocity = Vector2.ClampMagnitude(Rigidbody.linearVelocity, MaxSpeed);
            
            if (TargetVector.magnitude <= stopRadius)
            {
                if (_actionCooldownTimer <= 0)
                {
                    PerformStopAction();
                    return;
                }
                
                Rigidbody.linearVelocity *= Slowdown;
                
                _actionCooldownTimer--;
            }
            else if (TargetDirection.magnitude > 0)
            {
                float deviation = 1 + (TargetDirection - Rigidbody.linearVelocity.normalized).magnitude * _deviationFactor;
                Vector2 movementDirection = MathUtilities.RotateVector(TargetDirection, RandomAngle);

                float forceCoefficient = deviation * MoveForce * Mathf.Clamp(TargetVector.magnitude, 1, TargetVector.magnitude);
                Rigidbody.AddForce(movementDirection * forceCoefficient);
                
                _actionCooldownTimer = _stopRadiusMovementTimer;
                _stopRadiusMovementTimer = 0;
            }
        }

        private void PerformStopAction()
        {
            if (_stopRadiusMovementTimer <= 0)
            {
                float randomAngle = Random.value < 0.5 ?
                    Random.Range(-stopRadiusMovementAngle, -10f)
                    : Random.Range(10f, stopRadiusMovementAngle);
                
                _randomStopRadiusVector = MathUtilities.RotateVector(TargetDirection, randomAngle);
                _stopRadiusMovementTimer = stopRadiusMovementCooldown;
            }
            Rigidbody.linearVelocity = _randomStopRadiusVector.normalized * stopRadiusMoveForce;
            _stopRadiusMovementTimer--;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            EntityHealthController otherEntityHealth = 
                other.GetComponent<EntityHealthController>();

            if (otherEntityHealth && other.gameObject.CompareTag("Player"))
            {
                otherEntityHealth.CurrentHealth -= contactDamage;
            }
        }
        
        private void Rotate()
        {
            transform.up = Vector2.MoveTowards(
                transform.up, TargetDirection, 10 * Time.fixedDeltaTime);
        }
    }
}