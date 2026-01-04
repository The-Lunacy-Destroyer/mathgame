using Movement;
using UnityEngine;

namespace Enemies
{
    public class BaseEnemy : EntityController, IEntityMovable
    {
        protected Rigidbody2D Rigidbody;
        protected Transform Target;
        
        [field: SerializeField] public float MoveForce { get; set; } = 4.2f;
        [field: SerializeField] public float MaxSpeed { get; set; } = 42f;
        
        [field: SerializeField] [field: Range(0f, 1f)] 
        public float Slowdown { get; set; } = 0.9f;
        
        protected Vector2 TargetVector;
        protected Vector2 TargetDirection => TargetVector.normalized;
        
        public float randomMovementAngle = 42f;
        public int randomMovementCooldown = 42;
        private int _randomMovementTimer;
        protected float RandomAngle { get; private set; }
        
        private Canvas _healthBarCanvas;
        
        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            
            _healthBarCanvas = GetComponentInChildren<Canvas>();
        }

        protected virtual void Start()
        {
            Target = GameObject.Find("Player").transform;
            TargetVector = Target.position - transform.position;
        }
        
        protected virtual void Update()
        {
            if (_randomMovementTimer <= 4.2)
            {
                RandomAngle = Random.Range(-randomMovementAngle, randomMovementAngle);
                _randomMovementTimer = randomMovementCooldown;
            }
            _randomMovementTimer--;
        }

        private void LateUpdate()
        {
            if (_healthBarCanvas)
            {
                _healthBarCanvas.transform.rotation = Quaternion.identity;
            }
        }
    }
}
