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
        public GameObject SourceObject { get; set; }
        private bool _canDestroy = true;
        
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

        void OnTriggerStay2D(Collider2D other)
        {
            EntityHealthController otherEntityHealth = 
                other.GetComponent<EntityHealthController>();
            
            if (!SourceObject)
            {
                SourceObject = GetComponentInParent<EntityController>().gameObject;
                _canDestroy = false;
            }
            
            if (SourceObject && otherEntityHealth)
            {
                if (other.CompareTag("Enemy") && SourceObject.CompareTag("Player")
                    || 
                    other.CompareTag("Player") && SourceObject.CompareTag("Enemy"))
                {
                    DecreaseHealth(otherEntityHealth, _canDestroy);
                }
            }
        }

        void DecreaseHealth(EntityHealthController entityHealth, bool canDestroyObject)
        {
            entityHealth.CurrentHealth -= bulletDamage;
           
            if (entityHealth.CurrentHealth <= 0)
            {
                Destroy(entityHealth.gameObject);
            }
            if(canDestroyObject) Destroy(gameObject);
        }
    }
}
