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
        [field: SerializeField] [field: Range(0f, 1f)] 
        public float Slowdown { get; set; } = 0.9f;
        
        public float minSpeed = 2f;
        public float stopRadius = 6f;
        public float torqueForce = 30f;
        public float maxRotationSpeed = 50f;
        
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
        
        public float laserDamageScale = 1f;
        public float lasersTorqueForce = 0.6f;
        
        private int ActionDuration => 2 * lasersCooldown + lasersShootDuration;
        
        private Vector2 _targetVector;
        private Vector2 TargetDirection => _targetVector.normalized;

        private GameObject[] _lasers = new GameObject[3];
        private SpriteRenderer[] _laserRenderers =  new SpriteRenderer[3];
        private float _oldSpriteRendererXSize;
        
        private void Start()
        {
            _shootingSystem = GetComponent<EntityShootingController>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _target = GameObject.Find("Player").transform;

            for (int i = 0; i < 3; i++)
            {
                _lasers[i] = GameObject.Find($"Laser{i + 1}");
                _lasers[i].SetActive(false);
                _laserRenderers[i] =  _lasers[i].GetComponent<SpriteRenderer>();
                // if(i == 0)
                //     _oldSpriteRendererXSize = _laserRenderers[0].size.x;
                // _laserRenderers[i].size = new Vector2(_laserRenderers[i].size.x, 1);
            }
            
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
                
                if (_isActionActive)
                {
                    SlowMove();
                }
                else
                {
                    LaunchProjectiles();
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
                        ActivateLasers();
                        _isLasersActive = true;
                        _lasersCooldownTimer = lasersCooldown;
                    }
                }
                else
                {
                    ActivateLasersResize();
                    _lasersShootTimer--;
                
                    if (_lasersShootTimer <= 0)
                    {
                        _isLasersActive = false;
                        _lasersShootTimer = lasersShootDuration;
                    }
                }
            }
            else
            {
                DeactivateLasersResize();
            }
            
            _actionDurationTimer--;
            
            if (_actionDurationTimer <= 0)
            {
                DeactivateLasers();

                _isActionActive = false;
                _actionDurationTimer = ActionDuration;
            }
        }

        private void Move()
        {
            if (!_rigidbody) return;

            _rigidbody.linearVelocity = Vector2.ClampMagnitude(_rigidbody.linearVelocity, MaxSpeed);
            _rigidbody.angularVelocity = math.clamp(_rigidbody.angularVelocity, -maxRotationSpeed, maxRotationSpeed);

            if (_isActionActive)
            {
                SlowMove();
                return;
            }

            bool isStopRadius = _targetVector.magnitude <= stopRadius;
            if (_rigidbody.linearVelocity.magnitude > minSpeed && isStopRadius)
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
                    _rigidbody.totalTorque *= -2;
                }
            }

            _rigidbody.AddForce(_movementDirection * (deviation * MoveForce));
            _rigidbody.AddTorque(torqueForce);
            _randomMovementTimer--;
        }

        private void SlowMove()
        {
            if (_rigidbody.linearVelocity.magnitude > 0)
            {
                _rigidbody.linearVelocity *= Slowdown;
            }
            if (_rigidbody.linearVelocity.magnitude > minSpeed)
            {
                _rigidbody.angularVelocity *= Slowdown;
            }
            else
            {
                _rigidbody.AddTorque(lasersTorqueForce);
            }

        }

        private void LaunchProjectiles()
        {
            Vector2[][] positionsAndDirections = GetLaunchPositionsAndDirections();            
            _shootingSystem.ShootMany(
                positionsAndDirections[0], 
                positionsAndDirections[1]);
        }

        private void ActivateLasers()
        {
            for (int i = 0; i < 3; i++)
            {
                _lasers[i].SetActive(true);
            }
        }

        private void ActivateLasersResize()
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2 addVector = new Vector2(0, 0.5f);
                // if (_laserRenderers[i].size.x < _oldSpriteRendererXSize)
                // {
                //     addVector.x = 0.01f;
                // }
                _laserRenderers[i].size += addVector;
            }
        }
        private void DeactivateLasersResize()
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2 addVector = new Vector2(0, 0.5f);
                // if (_laserRenderers[i].size.x > 0f)
                // {
                //     addVector.x = 0.02f;
                // }
                _laserRenderers[i].size -= addVector;
            }
        }
        private void DeactivateLasers()
        {
            for (int i = 0; i < 3; i++)
            {
                _lasers[i].SetActive(false);
            }
        }
        
        private Vector2[][] GetLaunchPositionsAndDirections()
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
