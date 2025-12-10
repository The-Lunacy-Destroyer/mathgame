using System;
using System.Collections;
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
        
        private bool _canShoot = true;

        public void ShootMany(Vector2[] launchPosition, Vector2[] launchDirection)
        {
            if (!_canShoot) return;
            
            for (int i = 0; i < launchPosition.Length; i++)
            {
                StartCoroutine(LaunchProjectile(launchPosition[i], launchDirection[i]));
            }

            _canShoot = false;
        }
        public void Shoot(Vector2 launchPosition, Vector2 launchDirection)
        {
            if (!_canShoot) return;
            StartCoroutine(LaunchProjectile(launchPosition, launchDirection));
            _canShoot = false;
        }
        IEnumerator LaunchProjectile(Vector2 launchPosition, Vector2 launchDirection)
        {
            GameObject projectileObject = Instantiate(projectilePrefab, launchPosition, Quaternion.identity);
            ProjectileController projectile = projectileObject.GetComponent<ProjectileController>();
        
            projectile.SourceObject = gameObject;
            projectile.bulletDamage *= damageScale;
        
            float spread = Random.Range(-spreadAngle, spreadAngle); 
            launchDirection = MathUtilities.RotateVector(launchDirection, spread);
            projectile.Launch(launchDirection, projectileSpeed);
            
            yield return new WaitForSeconds(projectileCooldown);
            _canShoot = true;
        }
    }
}