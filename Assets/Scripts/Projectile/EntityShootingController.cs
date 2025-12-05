using Unity.Mathematics;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace Projectile
{
    public class EntityShootingController : MonoBehaviour
    {
        public GameObject projectilePrefab;
        
        public float projectileSpeed = 100f;
        public float damageScale = 1f;
        public float projectileCooldown = 0.5f;
        public float spreadAngle = 15f;
        
        private float _launchTimer;
        private bool _canLaunchProjectile;

        public void ShootMany(Vector2[] launchPosition, Vector2[] launchDirection)
        {
            if (!_canLaunchProjectile) return;

            for (int i = 0; i < math.min(launchPosition.Length, launchDirection.Length); i++)
            {
                LaunchProjectile(launchPosition[i], launchDirection[i]);
            }
            _canLaunchProjectile = false;
        }
        public void Shoot(Vector2 launchPosition, Vector2 launchDirection)
        {
            ShootMany(new []{launchPosition}, new []{launchDirection});
        }
        private void LaunchProjectile(Vector2 launchPosition, Vector2 launchDirection)
        {
            GameObject projectileObject = Instantiate(projectilePrefab, launchPosition, Quaternion.identity);
            ProjectileController projectile = projectileObject.GetComponent<ProjectileController>();
        
            projectile.SourceEntity = GetComponent<EntityController>();
            projectile.bulletDamage *= damageScale;
        
            float spread = Random.Range(-spreadAngle, spreadAngle); 
            launchDirection = MathUtilities.RotateVector(launchDirection, spread);
            projectile.Launch(launchDirection, projectileSpeed);
        }
        
        void Awake()
        {
            _launchTimer = projectileCooldown;
            _canLaunchProjectile = false;
        }
        
        void Update()
        {
            AddProjectileLaunchDelay();
        }

        void AddProjectileLaunchDelay()
        {
            _launchTimer -= Time.deltaTime;
            if (_launchTimer < 0)
            {
                _canLaunchProjectile = true;
                _launchTimer = projectileCooldown;
            }
        }
    }
}