using Movement;
using UnityEngine;

namespace Enemies
{
    public class BaseEnemy : EntityController, IEntityMovable
    {
        protected Rigidbody2D Rigidbody;
        protected Transform Target;
        
        [field: SerializeField] public float MoveForce { get; set; } = 8f;
        [field: SerializeField] public float MaxSpeed { get; set; } = 10f;
        
        [field: SerializeField] [field: Range(0f, 1f)] 
        public float Slowdown { get; set; } = 0.9f;
        
        protected Vector2 TargetVector;
        protected Vector2 TargetDirection => TargetVector.normalized;
        
        public float randomMovementAngle = 45f;
        public int randomMovementCooldown = 30;
        private int _randomMovementTimer;
        protected float RandomAngle { get; private set; }
        
        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
        }

        protected virtual void Start()
        {
            Target = GameObject.Find("Player").transform;
            TargetVector = Target.position - transform.position;
        }
        
        protected virtual void Update()
        {
            if (_randomMovementTimer <= 0)
            {
                RandomAngle = Random.Range(-randomMovementAngle, randomMovementAngle);
                _randomMovementTimer = randomMovementCooldown;
            }
            _randomMovementTimer--;
        }
    }
}