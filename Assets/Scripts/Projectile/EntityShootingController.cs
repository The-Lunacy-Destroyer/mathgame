using UnityEngine;
using Utilities;

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
        private bool _canLaunchProjectile = true;

        public void LaunchProjectile(Vector2 launchPosition, Vector2 launchDirection)
        {
            if (_canLaunchProjectile)
            {
                GameObject projectileObject = Instantiate(projectilePrefab, launchPosition, Quaternion.identity);
                ProjectileController projectile = projectileObject.GetComponent<ProjectileController>();
            
                projectile.Entity = GetComponent<EntityController>();
                projectile.bulletDamage *= damageScale;
                _canLaunchProjectile = false;
            
                float spread = Random.Range(-spreadAngle, spreadAngle); 
                launchDirection = MathUtilities.RotateVector(launchDirection, spread);
                projectile.Launch(launchDirection, projectileSpeed);
            }
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