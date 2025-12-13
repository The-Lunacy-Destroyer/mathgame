using System;
using Health;
using UnityEngine;

namespace Projectile
{
    public class BulletController : ProjectileController
    {
        private Rigidbody2D _rigidbody;

        public GameObject flashPrefab;
        public GameObject shootingPrefab;
        public float shootingOffset = 0.1f;
        
        private void Awake()
        {
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

        protected override void OnTriggerStay2D(Collider2D other)
        {
            base.OnTriggerStay2D(other);
            if (other.gameObject.layer != LayerMask.NameToLayer("Projectiles")
                && !other.gameObject.CompareTag(SourceObject.tag)) 
                Destroy(gameObject);
        }
    }
}