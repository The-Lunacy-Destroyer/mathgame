using System;
using Health;
using UnityEngine;

namespace Projectile
{
    public class BulletController : ProjectileController
    {
        private Rigidbody2D _rigidbody;
        private Camera _camera;

        public GameObject flashPrefab;
        public GameObject shootingPrefab;
        public float shootingOffset = 0.1f;
        
        private void Awake()
        {
            _camera = Camera.main;
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            if (SourceObject)
            {
                Vector2 position = transform.position + SourceObject.transform.up * shootingOffset;
                GameObject shooting = Instantiate(shootingPrefab, position, Quaternion.identity);
                shooting.transform.up = SourceObject.transform.up;
            }
        }
        
        public void Launch(Vector2 direction, float force)
        {
            _rigidbody.AddForce(direction * force);
            transform.up = direction;
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject == SourceObject) return;
            
            GameObject flash = Instantiate(flashPrefab, transform.position, Quaternion.identity);
            flash.transform.up = -transform.up;
        }
    }
}