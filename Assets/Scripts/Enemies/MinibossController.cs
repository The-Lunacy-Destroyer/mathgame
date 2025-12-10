using Health;
using Movement;
using Projectile;
using Unity.Mathematics;
using Unity.Mathematics.Geometry;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class MinibossController : BaseEnemy
    {
        private EntityShootingController _shootingSystem;

        public float minSpeed = 2f;
        public float stopRadius = 6f;
        public float torqueForce = 30f;
        public float maxRotationSpeed = 50f;
        
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
        
        public float lasersTorqueForce = 0.6f;
        public float contactDamage = 35f;
        
        private int ActionDuration => 2 * lasersCooldown + lasersShootDuration;

        private GameObject[] _lasers = new GameObject[3];
        private SpriteRenderer[] _laserRenderers =  new SpriteRenderer[3];
        private float _oldSpriteRendererXSize;

        protected override void Awake()
        {
            base.Awake();
            _shootingSystem = GetComponent<EntityShootingController>();
        }
        protected override void Start()
        {
            base.Start();

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
            if (Target)
            {
                TargetVector = Target.position - transform.position;
                
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
            if (!_isActionActive) // до начала специальной атаки
            {
                _actionCooldownTimer--;
                
                if (_actionCooldownTimer <= 0)
                {
                    _isActionActive = true;
                    _actionCooldownTimer = actionCooldown;
                }

                return;
            }
            
            if (_actionDurationTimer > lasersCooldown) // период спец. атаки до деактивации лазеров
            {
                if (!_isLasersActive) // до активации лазеров
                {
                    _lasersCooldownTimer--;
                
                    if (_lasersCooldownTimer <= 0)
                    {
                        ActivateLasers();
                        _isLasersActive = true;
                        _lasersCooldownTimer = lasersCooldown;
                    }
                }
                else // во время активации лазеров
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
            else // деактивация лазеров
            {
                DeactivateLasersResize();
            }
            
            _actionDurationTimer--;
            
            if (_actionDurationTimer <= 0) // конец спец. атаки
            {
                DeactivateLasers();

                _isActionActive = false;
                _actionDurationTimer = ActionDuration;
            }
        }

        private void Move()
        {
            if (!Rigidbody) return;

            Rigidbody.linearVelocity = Vector2.ClampMagnitude(Rigidbody.linearVelocity, MaxSpeed);
            Rigidbody.angularVelocity = math.clamp(Rigidbody.angularVelocity, -maxRotationSpeed, maxRotationSpeed);

            if (_isActionActive)
            {
                SlowMove();
                return;
            }

            if (Rigidbody.linearVelocity.magnitude > minSpeed 
                && TargetVector.magnitude <= stopRadius)
            {
                Rigidbody.linearVelocity *= Slowdown;
            }
            float deviation = 1 + (TargetDirection - Rigidbody.linearVelocity.normalized).magnitude;

            Vector2 movementDirection = MathUtilities.RotateVector(TargetDirection, RandomAngle);
            Rigidbody.AddForce(movementDirection * (deviation * MoveForce));
            Rigidbody.AddTorque(torqueForce);
        }
        
        private void SlowMove()
        {
            if (Rigidbody.linearVelocity.magnitude > 0)
            {
                Rigidbody.linearVelocity *= Slowdown;
            }
            if (Rigidbody.linearVelocity.magnitude > minSpeed)
            {
                Rigidbody.angularVelocity *= Slowdown;
            }
            else
            {
                Rigidbody.AddTorque(lasersTorqueForce);
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
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            EntityHealthController otherEntityHealth = 
                collision.collider.GetComponent<EntityHealthController>();
            Debug.Log(otherEntityHealth);

            if (otherEntityHealth && collision.gameObject.CompareTag("Player"))
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
