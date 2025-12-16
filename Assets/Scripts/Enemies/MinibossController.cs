using EntityAI;
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
        private EntityBulletController _bulletSystem;
        private ActionController _actionController;

        private GameObject _laserStartSound;
        private GameObject _laserEndSound;
        
        public float minSpeed = 2f;
        public float stopRadius = 6f;
        public float torqueForce = 30f;
        public float maxRotationSpeed = 50f;
        
        public float specialTorqueForce = 0.66f;
        public float contactDamage = 35f;

        public float minProjectileCooldown = 0.1f;
        public float minProjectileSpeed = 100f;
        private float _initialProjectileCooldown;
        private float _initialProjectileSpeed;

        public float secondActionTimerScale = 1.75f;
        public float secondActionPostTimerScale = 2.3f;
        
        public float laserMaxSize = 50f;
        public float deviationForce = 3f;
        private float _laserSize;
        
        private GameObject[] _lasers = new GameObject[3];
        private SpriteRenderer[] _laserRenderers =  new SpriteRenderer[3];

        private bool _isLasersAction;
        
        protected override void Awake()
        {
            base.Awake();
            _bulletSystem = GetComponent<EntityBulletController>();
            _actionController = GetComponent<ActionController>();
        }
        protected override void Start()
        {
            base.Start();

            _laserStartSound = transform.Find("LaserStart").gameObject;
            _laserEndSound = transform.Find("LaserEnd").gameObject;
            
            for (int i = 0; i < 3; i++)
            {
                _lasers[i] = transform.Find($"Laser{i + 1}").gameObject;
                _lasers[i].SetActive(false);
                _laserRenderers[i] = _lasers[i].GetComponent<SpriteRenderer>();
                _initialProjectileCooldown = _bulletSystem.projectileCooldown;
                _initialProjectileSpeed = _bulletSystem.projectileSpeed;
            }
            
            _actionController.OnDefaultStart += OnDefaultStart;
            _actionController.OnPreSpecialStart += OnPreSpecialStart;
            _actionController.OnSpecialStart += OnSpecialStart;
            _actionController.OnPostSpecialStart += OnPostSpecialStart;
            
            _actionController.OnDefault += Move;
            _actionController.OnPreSpecial += PreSpecialMove;
            _actionController.OnSpecial += () =>
            {
                if (_isLasersAction) SpecialMove(specialTorqueForce);
                else SpecialMove(specialTorqueForce * 2f);
            };
            _actionController.OnPostSpecial += PostSpecialMove;
            
            _actionController.OnDefault += LaunchProjectiles;
            _actionController.OnPreSpecial += () =>
            {
                if (!_isLasersAction) LaunchProjectiles();
            };
            _actionController.OnSpecial += () =>
            {
                if (_isLasersAction) ActivateLasers();
                else LaunchProjectiles();
            };
            _actionController.OnPostSpecial += () =>
            {
                if (_isLasersAction) DeactivateLasers();
            };
        }
        
        private void FixedUpdate()
        {
            if (Target) TargetVector = Target.position - transform.position;
        }
        
        private void OnDefaultStart()
        {
            if (Rigidbody.angularVelocity > 0)
            {
                Rigidbody.totalTorque = 0;
                Rigidbody.angularVelocity = 0;
            }
            if (_isLasersAction)
            {
                for (int i = 0; i < 3; i++)
                {
                    _lasers[i].SetActive(false);
                }
            }
            _isLasersAction = !_isLasersAction;
        }

        private void OnPreSpecialStart()
        {
            _laserStartSound.SetActive(true);
            _laserEndSound.SetActive(false);
        }
        private void OnSpecialStart()
        {
            if (!_isLasersAction)
            {
                _actionController.AITimer = Mathf.RoundToInt(secondActionTimerScale * _actionController.AITimer);
                _bulletSystem.projectileCooldown = minProjectileCooldown;
                _bulletSystem.projectileSpeed = minProjectileSpeed;
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    _lasers[i].SetActive(true);
                }
            }
                
            if (Rigidbody.linearVelocity.magnitude > 0)
            {
                Rigidbody.totalForce = Vector2.zero;
                Rigidbody.linearVelocity = Vector2.zero;
            }
            if (Rigidbody.angularVelocity > 0)
            {
                Rigidbody.totalTorque = 0;
                Rigidbody.angularVelocity = 0;
            }
        }
        private void OnPostSpecialStart()
        {
            if (!_isLasersAction)
            {
                _actionController.AITimer = Mathf.RoundToInt(secondActionPostTimerScale * _actionController.AITimer);
                _bulletSystem.projectileCooldown = _initialProjectileCooldown;
                _bulletSystem.projectileSpeed = _initialProjectileSpeed;
            }
            else
            {
                _laserSize = _laserRenderers[0].size.y;
                _laserStartSound.SetActive(false);
                _laserEndSound.SetActive(true);
            }
        }
        
        private void Move()
        {
            if (!Rigidbody) return;

            Rigidbody.linearVelocity = Vector2.ClampMagnitude(Rigidbody.linearVelocity, MaxSpeed);
            Rigidbody.angularVelocity = math.clamp(Rigidbody.angularVelocity, -maxRotationSpeed, maxRotationSpeed);

            if (Rigidbody.linearVelocity.magnitude > minSpeed 
                && TargetVector.magnitude <= stopRadius)
            {
                Rigidbody.linearVelocity *= Slowdown;
            }
            float deviation = 1 + (TargetDirection - Rigidbody.linearVelocity.normalized).magnitude * deviationForce;

            Vector2 movementDirection = MathUtilities.RotateVector(TargetDirection, RandomAngle);
            Rigidbody.AddForce(movementDirection * (deviation * MoveForce));
            Rigidbody.AddTorque(torqueForce);
        }
        private void PreSpecialMove()
        {
            Rigidbody.AddTorque(-Rigidbody.angularVelocity / _actionController.AITimer);
            Rigidbody.AddForce(-Rigidbody.linearVelocity / _actionController.AITimer);
        }
        private void PostSpecialMove()
        {
            Rigidbody.AddTorque(-Rigidbody.angularVelocity / _actionController.AITimer);
            Rigidbody.AddForce(-Rigidbody.linearVelocity / _actionController.AITimer);
        }
        private void SpecialMove(float tForce)
        {
            Rigidbody.AddTorque(tForce);
        }
        private void LaunchProjectiles()
        {
            Vector2[][] positionsAndDirections = GetLaunchPositionsAndDirections();            
            _bulletSystem.ShootMany(
                positionsAndDirections[0], 
                positionsAndDirections[1]);
        }
        private void ActivateLasers()
        {
            if (_laserRenderers[0].size.y > laserMaxSize) return;
            
            for (int i = 0; i < 3; i++)
            {
                Vector2 addVector = new Vector2(0, 1f);
                // if (_laserRenderers[i].size.x < _oldSpriteRendererXSize)
                // {
                //     addVector.x = 0.01f;
                // }
                _laserRenderers[i].size += addVector;
            }
        }
        private void DeactivateLasers()
        {
            if (_laserRenderers[0].size.y == 0) return;
            
            for (int i = 0; i < 3; i++)
            {
                Vector2 addVector = new Vector2(0, (_laserSize + 2f) / _actionController.aiDurations[3]);
                
                Vector2 newSize = _laserRenderers[i].size - addVector;
                if (newSize.y < 0) newSize = new Vector2(_laserRenderers[i].size.x, 0);

                // if (_laserRenderers[i].size.x > 0f)
                // {
                //     addVector.x = 0.02f;
                // }
                _laserRenderers[i].size = newSize;
            }
        }

        // private void StartBulletHell()
        // {
        //     float t = (float)(_actionController.aiDurations[1] - _actionController.AITimer) /
        //               _actionController.aiDurations[1];
        //     _shootingSystem.projectileCooldown = Mathf.Lerp(_initialProjectileCooldown, minProjectileCooldown, t);
        //     _shootingSystem.projectileSpeed = Mathf.Lerp(_initialProjectileSpeed, minProjectileSpeed, t);
        // }
        //
        // private void EndBulletHell()
        // {
        //     float t = (float)(_actionController.aiDurations[3] - _actionController.AITimer) /
        //                                     _actionController.aiDurations[3];
        //     _shootingSystem.projectileCooldown = Mathf.Lerp(minProjectileCooldown, _initialProjectileCooldown, t);
        //     _shootingSystem.projectileSpeed = Mathf.Lerp(minProjectileSpeed, _initialProjectileSpeed, t);
        // }
        
        private Vector2[][] GetLaunchPositionsAndDirections()
        {
            float a = Rigidbody.angularVelocity * (1.4f - _bulletSystem.projectileCooldown * 2);
            
            Vector2 dir1 = transform.up.normalized;
            Vector2 dir2 = MathUtilities.RotateVector(dir1, 120);
            Vector2 dir3 = MathUtilities.RotateVector(dir1, -120);

            Vector2 pos1 = (Vector2)transform.position + dir1 * 0.66f;
            Vector2 pos2 = (Vector2)transform.position + MathUtilities.RotateVector(dir2, 15) * 1.25f;
            Vector2 pos3 = (Vector2)transform.position + MathUtilities.RotateVector(dir3, -15) * 1.25f;

            return new[]
            {
                new[] { pos1, pos2, pos3 },
                new[] { 
                    MathUtilities.RotateVector(dir1, a), 
                    MathUtilities.RotateVector(dir2, a), 
                    MathUtilities.RotateVector(dir3, a)
                }
            };
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            EntityHealthController otherEntityHealth = 
                other.collider.GetComponent<EntityHealthController>();

            if (otherEntityHealth && other.gameObject.CompareTag("Player"))
            {
                otherEntityHealth.CurrentHealth -= contactDamage;
            }
        }

    }
}
