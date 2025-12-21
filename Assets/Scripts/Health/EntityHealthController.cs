using UnityEngine;

namespace Health
{
    public class EntityHealthController : MonoBehaviour
    {
        public GameObject deathSoundPrefab;

        private FlashMaskController _flashMask;
        private HealthBarController _healthBar;

        public float maxHealth = 100.0f;
        
        private float _currentHealth;
        public virtual float CurrentHealth
        {
            get => _currentHealth;
            set
            {
                if (value <= 0)
                {
                    GameObject deathSound = Instantiate(deathSoundPrefab, transform.position, Quaternion.identity);
                    Destroy(deathSound, 1f);
                    Destroy(transform.gameObject);
                }
                else if (value < _currentHealth)
                {
                    _flashMask?.Flash();
                    
                    _healthBar.Entity.Shake();
                }
                _currentHealth = Mathf.Clamp(value, 0, maxHealth);
                _healthBar?.UpdateHealthBar(CurrentHealth, maxHealth);
            }
        }

        protected virtual void Start()
        {
            _flashMask = GetComponent<FlashMaskController>();

            _healthBar = GetComponentInChildren<HealthBarController>();
            _healthBar.Entity = GetComponent<EntityController>();
            CurrentHealth = maxHealth;
        }
    }
}