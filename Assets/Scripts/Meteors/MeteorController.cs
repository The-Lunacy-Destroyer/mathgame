using System;
using Health;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace Meteors
{
    public class MeteorController : EntityController
    {
        private Rigidbody2D _rigidbody;
        private EntityHealthController _healthSystem;
        
        public float torqueMaxScale = 2f;
        public float forceMaxScale = 2f;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _healthSystem = GetComponent<EntityHealthController>();
        }

        private void Start()
        {
            _healthSystem.maxHealth *= _rigidbody.mass;
            _healthSystem.CurrentHealth = _healthSystem.maxHealth;
            
            float torqueScale = Random.Range(-torqueMaxScale, torqueMaxScale);
            float forceScale = Random.Range(-forceMaxScale, forceMaxScale);
            float forceAngle = Random.Range(-180f, 180f);
            
            _rigidbody.AddTorque(torqueScale, ForceMode2D.Impulse);
            _rigidbody.AddForce(
                MathUtilities.RotateVector(Vector2.up, forceAngle) * forceScale, 
                ForceMode2D.Impulse);
        }
    }
}
