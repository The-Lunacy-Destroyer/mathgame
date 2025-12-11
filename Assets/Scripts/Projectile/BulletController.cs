using Health;
using UnityEngine;

namespace Projectile
{
    public class BulletController : ProjectileController
    {
        private Rigidbody2D _rigidbody;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
            _rigidbody = GetComponent<Rigidbody2D>();
        }
        
        public void Launch(Vector2 direction, float force)
        {
            _rigidbody.AddForce(direction * force);
        }
        
        private void Update()
        {
            if ((_camera.transform.position - transform.position).magnitude > 40.0f)
            {
                Destroy(gameObject);
            }
        }

        protected override void DecreaseHealth(EntityHealthController entityHealth)
        {
            base.DecreaseHealth(entityHealth);
            Destroy(gameObject);
        }
    }
}