using Movement;
using Projectile;
using Unity.Mathematics;
using Unity.Mathematics.Geometry;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class MinibossController : EntityController, IEntityMovable
    {
        private Transform _target;
        private EntityShootingController _shootingSystem;
        private Rigidbody2D _rigidbody;
    
        [field: SerializeField] public float MoveForce { get; set; } = 8f;
        [field: SerializeField] public float MaxSpeed { get; set; } = 10f;
        [field: SerializeField] public float MinSpeed { get; set; } = 2f;
        [field: SerializeField] public float Slowdown { get; set; } = 0.9f;
        [field: SerializeField] public float StopRadius { get; set; } = 6f;
        [field: SerializeField] public float TorqueForce { get; set; } = 30f;
        [field: SerializeField] public float MaxRotationSpeed { get; set; } = 50f;

        public float randomMovementAngle = 45f;
        public int randomMovementCooldown = 30;
        private int _randomMovementTimer = 0;
        private Vector2 _movementDirection;
        
        [Min(1)]
        public int actionCooldown = 300;
        private int _actionCooldownTimer;
        private int _actionDurationTimer;
        private bool _isActionActive;

        public int lasersCooldown = 5;
        private int _lasersCooldownTimer;
        private bool _isLasersActive;

        public int lasersShootDuration = 50;
        private int _lasersShootTimer;
        
        private int ActionDuration => 2 * lasersCooldown + lasersShootDuration;
        
        private Vector2 _targetVector;
        private Vector2 TargetDirection => _targetVector.normalized;
        
        private void Start()
        {
            _shootingSystem = GetComponent<EntityShootingController>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _target = GameObject.Find("Player").transform;
            
            _actionCooldownTimer = actionCooldown;
            _actionDurationTimer = ActionDuration;
            _lasersCooldownTimer = lasersCooldown;
            _lasersShootTimer = lasersShootDuration;
        }
        
        private void FixedUpdate()
        {
            if (_target)
            {
                _targetVector = _target.position - transform.position;
                
                ControlActions();

                if (_isLasersActive)
                {
                    LaunchLasers();
                }
                else
                {
                    LaunchProjectiles();
                }
                if (_isActionActive)
                {
                    SlowMove();
                }
                else
                {
                    Move();
                }
            }
        }

        private void ControlActions()
        {
            if (!_isActionActive)
            {
                _actionCooldownTimer--;
                
                if (_actionCooldownTimer <= 0)
                {
                    _isActionActive = true;
                    _actionCooldownTimer = actionCooldown;
                }

                return;
            }
            
            if (_actionDurationTimer > lasersCooldown)
            {
                if (!_isLasersActive)
                {
                    _lasersCooldownTimer--;
                
                    if (_lasersCooldownTimer <= 0)
                    {
                        _isLasersActive = true;
                        _shootingSystem.projectileCooldown = 0.01f;
                        _lasersCooldownTimer = lasersCooldown;
                    }
                }
                else
                {
                    _lasersShootTimer--;
                
                    if (_lasersShootTimer <= 0)
                    {
                        _isLasersActive = false;
                        _shootingSystem.projectileCooldown = 0.5f;
                        _lasersShootTimer = lasersShootDuration;
                    }
                }
            }
            
            _actionDurationTimer--;
            
            if (_actionDurationTimer <= 0)
            {
                _isActionActive = false;
                _actionDurationTimer = ActionDuration;
            }
        }

        private void Move()
        {
            if (!_rigidbody) return;

            _rigidbody.linearVelocity = Vector2.ClampMagnitude(_rigidbody.linearVelocity, MaxSpeed);
            _rigidbody.angularVelocity = math.clamp(_rigidbody.angularVelocity, -MaxRotationSpeed, MaxRotationSpeed);

            if (_isActionActive)
            {
                SlowMove();
                return;
            }

            bool isStopRadius = _targetVector.magnitude <= StopRadius;
            if (_rigidbody.linearVelocity.magnitude > MinSpeed && isStopRadius)
            {
                _rigidbody.linearVelocity *= Slowdown;
            }
            float deviation = 1 + (TargetDirection - _rigidbody.linearVelocity.normalized).magnitude;
            if (_randomMovementTimer <= 0)
            {
                _movementDirection = MathUtilities.RotateVector(TargetDirection, 
                    Random.Range(-randomMovementAngle, randomMovementAngle));

                _randomMovementTimer = randomMovementCooldown;
                if (!isStopRadius)
                {
                    _rigidbody.totalTorque *= -1;
                }
            }

            _rigidbody.AddForce(_movementDirection * (deviation * MoveForce));
            _rigidbody.AddTorque(TorqueForce);
            _randomMovementTimer--;
        }

        private void SlowMove()
        {
            if (_rigidbody.linearVelocity.magnitude > MinSpeed)
            {
                _rigidbody.linearVelocity *= Slowdown;
            }

            _rigidbody.angularVelocity *= Slowdown;
        }

        private void LaunchProjectiles()
        {
            Vector2[][] directionsAndPositions = GetLaunchDirectionsAndPositions();            
            _shootingSystem.ShootMany(
                directionsAndPositions[0], 
                directionsAndPositions[1]);
        }
        

        private void LaunchLasers()
        {
            Vector2[][] directionsAndPositions = GetLaunchDirectionsAndPositions();            
            _shootingSystem.ShootMany(
                directionsAndPositions[0], 
                directionsAndPositions[1]);
        }
        
        private Vector2[][] GetLaunchDirectionsAndPositions()
        {
            Vector2 dir1 = transform.up.normalized;
            Vector2 dir2 = MathUtilities.RotateVector(dir1, 120);
            Vector2 dir3 = MathUtilities.RotateVector(dir1, -120);

            Vector2 pos1 = (Vector2)transform.position + dir1 * 0.66f;
            Vector2 pos2 = (Vector2)transform.position + MathUtilities.RotateVector(dir2, 15) * 1.25f;
            Vector2 pos3 = (Vector2)transform.position + MathUtilities.RotateVector(dir3, -15) * 1.25f;

            return new[]
            {
                new[] { pos1, pos2, pos3 },
                new[] { dir1, dir2, dir3 }
            };
        }

    }
}
