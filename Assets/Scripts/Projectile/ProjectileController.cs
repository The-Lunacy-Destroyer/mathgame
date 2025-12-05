using Enemies;
using Health;
using UnityEngine;

namespace Projectile
{
    public class ProjectileController : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        private Camera _camera;
    
        public float bulletDamage = 20.1f;
        public EntityController SourceEntity { get; set; }
    
        public void Launch(Vector2 direction, float force)
        {
            _rigidbody.AddForce(direction * force);
        }
        
        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _camera = Camera.main;
        }

        void Update()
        {
            if ((_camera.transform.position - transform.position).magnitude > 40.0f)
            {
                Destroy(gameObject);
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            EntityController otherEntity = other.GetComponent<EntityController>();
            EntityHealthController otherEntityHealth = 
                otherEntity?.GetComponent<EntityHealthController>();

            if (SourceEntity && otherEntity && otherEntityHealth)
            {
                if (otherEntity.gameObject.CompareTag("Enemy")
                    &&
                    SourceEntity.gameObject.CompareTag("Player")
                    || 
                    otherEntity.gameObject.CompareTag("Player")
                    &&
                    SourceEntity.gameObject.CompareTag("Enemy"))
                {
                    DecreaseHealth(otherEntityHealth);
                }
            }
        }

        void DecreaseHealth(EntityHealthController entityHealth)
        {
            entityHealth.CurrentHealth -= bulletDamage;
           
            if (entityHealth.CurrentHealth <= 0)
            {
                Destroy(entityHealth.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
