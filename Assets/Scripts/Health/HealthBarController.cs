using UnityEngine;
using UnityEngine.UI;

namespace Health
{
    public class HealthBarController : MonoBehaviour
    {
        private Camera _camera;
    
        public Slider slider;
        public Vector2 offset;
    
        public EntityController Entity { get; set; }
    
        public void UpdateHealthBar(float currentHealth, float maxHealth)
        {
            gameObject.SetActive(currentHealth < maxHealth);
            slider.value = currentHealth / maxHealth;
        }

        void Awake()
        {
            _camera = Camera.main;
        }
        
        void LateUpdate()
        {
            if (Entity)
            {
                transform.rotation = _camera.transform.rotation;
                Vector2 pos = Entity.transform.position;
                transform.position = pos - offset;
            }
        }
    }
}
