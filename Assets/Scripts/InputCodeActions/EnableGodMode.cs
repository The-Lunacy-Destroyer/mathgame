using Health;
using Projectile;
using UnityEngine;

namespace InputCodeActions
{
    public class EnableGodMode : MonoBehaviour
    {
        private EntityBulletController _bulletSystem;
        private EntityHealthController _healthSystem;
        
        private float _oldProjectileCooldown;
        private float _oldProjectileSpeed;
        private float _oldDamageScale;
        private float _oldMaxHealth;
        private float _oldSpreadAngle;
        
        private void Start()
        {
            _bulletSystem = GetComponent<EntityBulletController>();
            _healthSystem = GetComponent<EntityHealthController>();
            
            _oldProjectileCooldown = _bulletSystem.projectileCooldown;
            _oldProjectileSpeed = _bulletSystem.projectileSpeed;
            _oldDamageScale = _bulletSystem.damageScale;
            _oldMaxHealth = _healthSystem.maxHealth;
            _oldSpreadAngle = _bulletSystem.spreadAngle;
        }

        private void OnEnable()
        {
            if (_bulletSystem)
            {
                _bulletSystem.projectileCooldown = 0.001f;
                _bulletSystem.projectileSpeed = 900f;
                _bulletSystem.damageScale = 100f;
                _bulletSystem.spreadAngle = 40f;
            }

            if (_healthSystem)
            {
                _healthSystem.maxHealth = 100000;
                _healthSystem.CurrentHealth = 100000;
            }
        }

        private void OnDisable()
        {
            if (_bulletSystem)
            {
                _bulletSystem.projectileCooldown = _oldProjectileCooldown;
                _bulletSystem.projectileSpeed = _oldProjectileSpeed;
                _bulletSystem.damageScale = _oldDamageScale;
                _bulletSystem.spreadAngle = _oldSpreadAngle;
            }

            if (_healthSystem)
            {
                _healthSystem.maxHealth = _oldMaxHealth;
                _healthSystem.CurrentHealth = _oldMaxHealth;
            }
        }
    }
}
