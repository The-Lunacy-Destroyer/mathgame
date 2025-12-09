using UnityEngine;

namespace Health
{
    public class EntityHealthController : MonoBehaviour
    {
        private HealthBarController _healthBar;

        public float maxHealth = 100.0f;
        
        private float _currentHealth;
        public float CurrentHealth
        {
            get => _currentHealth;
            set
            {
                _currentHealth = Mathf.Clamp(value, 0, maxHealth);
                _healthBar.UpdateHealthBar(CurrentHealth, maxHealth);
            }
        }

        void Start()
        {
            _healthBar = GetComponentInChildren<HealthBarController>();
            _healthBar.Entity = GetComponent<EntityController>();
            CurrentHealth = maxHealth;
        }
    }
}