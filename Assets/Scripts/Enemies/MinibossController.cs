using Projectile;
using UnityEngine;

namespace Enemies
{
    public class MinibossController : EntityController
    {
        private Transform _gun1;
        private Transform _gun2;
        private Transform _gun3;
        private Transform _center;
        private Transform _target;
        private EntityShootingController _shootingSystem;
        private Rigidbody2D _rigidbody;
    
        private float _rotationAngle = 0;
        private float _positionAngle = 0;
    
        void Start()
        {
            _shootingSystem = GetComponent<EntityShootingController>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _target = GameObject.Find("Player").transform;
            _gun1 = transform.Find("gun1");
            _gun2 = transform.Find("gun2");
            _gun3 = transform.Find("gun3");
            _center = transform;

        }
        
        void FixedUpdate()
        {
            if (_target)
            {
                LaunchProjectiles();
                
                _rotationAngle += 0.1f;
                _positionAngle += 0.05f;
                float dx = transform.position.x - _target.position.x;
                float dy = transform.position.y - _target.position.y;
                float distance = Mathf.Sqrt(dx * dx + dy * dy);
                _rigidbody.transform.up = 
                    new Vector3(Mathf.Cos(_rotationAngle), Mathf.Sin(_rotationAngle), 0);
                _rigidbody.position = _target.position + 
                                      new Vector3(Mathf.Sin(_positionAngle), Mathf.Cos(_positionAngle)) * distance;
            }
        }

        void LaunchProjectiles()
        {
            Vector2 vec1 = (_gun1.position - _center.position).normalized;
            Vector2 vec2 = (_gun2.position - _center.position).normalized;
            Vector2 vec3 = (_gun3.position - _center.position).normalized;

            _shootingSystem.ShootMany(
                new Vector2[]{_gun1.position, _gun2.position, _gun3.position}, 
                new [] {vec1, vec2, vec3});
        }
    }
}
