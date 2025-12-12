using Movement;
using Projectile;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class Enemy1Controller : BaseEnemy
    {
        private EntityBulletController _bulletSystem;
        public float stopRadius = 5f;
        public float minStopRadiusScale = 0.75f;
        public float maxStopRadiusScale = 1.25f;
        
        public float minDeviation = 0.95f;
        public float maxDeviation = 1.25f;
        private float _deviationFactor;

        [Min(1)]
        public int actionCooldown = 1;
        private int _actionCooldownTimer;

        public int stopRadiusMovementCooldown = 30;
        private int _stopRadiusMovementTimer;
        private Vector2 _randomStopRadiusVector;
        
        public float shootRadius = 8f;

        protected override void Awake()
        {
            base.Awake();
            _bulletSystem = GetComponent<EntityBulletController>();
            
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
                
                if (_bulletSystem&& TargetVector.magnitude <= shootRadius)
                {
                    _bulletSystem.Shoot(transform.position + transform.up * 0.25f, transform.up);
                }
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
                Rigidbody.AddForce(movementDirection * (deviation * MoveForce));
                
                _actionCooldownTimer = actionCooldown;
            }
        }

        private void PerformStopAction()
        {
            if (_stopRadiusMovementTimer <= 0)
            {
                float randomAngle = Random.value < 0.5f ?
                    Random.Range(-50f, -90f) : 
                    Random.Range(50f, 90f);
                
                _randomStopRadiusVector = MathUtilities.RotateVector(TargetDirection, randomAngle);
                _stopRadiusMovementTimer = stopRadiusMovementCooldown;
            }
            Rigidbody.linearVelocity = _randomStopRadiusVector.normalized * MoveForce;
            _stopRadiusMovementTimer--;
        }
        
        private void Rotate()
        {
            transform.up = Vector2.MoveTowards(
                transform.up, TargetDirection, 10 * Time.fixedDeltaTime);
        }
    }
}
