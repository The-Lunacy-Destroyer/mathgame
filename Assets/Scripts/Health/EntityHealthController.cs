using UnityEngine;

namespace Health
{
    public class EntityHealthController : MonoBehaviour
    {
        private FlashMaskController _flashMask;
        private HealthBarController _healthBar;

        public float maxHealth = 100.0f;
        
        private float _currentHealth;
        public float CurrentHealth
        {
            get => _currentHealth;
            set
            {
                if (value < _currentHealth)
                {
                    _flashMask?.Flash();
                    _healthBar.Entity.Shake();
                }
                _currentHealth = Mathf.Clamp(value, 0, maxHealth);
                _healthBar.UpdateHealthBar(CurrentHealth, maxHealth);
            }
        }

        void Start()
        {
            _flashMask = GetComponent<FlashMaskController>();

            _healthBar = GetComponentInChildren<HealthBarController>();
            _healthBar.Entity = GetComponent<EntityController>();
            CurrentHealth = maxHealth;
        }
    }
}