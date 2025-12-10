using Projectile;
using UnityEngine;
using Utilities;

namespace Enemies
{
    public class MinibossController : EntityController
    {
        private Transform _center;
        private Transform _target;
        private EntityShootingController _shootingSystem;
        private Rigidbody2D _rigidbody;
    
        private float _rotationAngle = 0;
        private float _positionAngle = 0;
    
        void Start()
        {
           // gameObject.SetActive(false);
            _shootingSystem = GetComponent<EntityShootingController>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _target = GameObject.Find("Player").transform;
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
            Vector2 vec1 = transform.up.normalized;
            Vector2 vec2 = MathUtilities.RotateVector(vec1, 120);
            Vector2 vec3 = MathUtilities.RotateVector(vec1, -120);

            Vector2 pos1 = (Vector2)_center.position + vec1 * 0.66f;
            Vector2 pos2 = (Vector2)_center.position + MathUtilities.RotateVector(vec2, 15) * 1.25f;
            Vector2 pos3 = (Vector2)_center.position + MathUtilities.RotateVector(vec3, -15) * 1.25f;
            
            _shootingSystem.ShootMany(
                new []{pos1, pos2, pos3}, 
                new [] {vec1, vec2, vec3});
        }
    }
}
