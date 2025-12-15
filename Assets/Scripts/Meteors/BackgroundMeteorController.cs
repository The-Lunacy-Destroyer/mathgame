using UnityEngine;
using Utilities;

namespace Meteors
{
    public class BackgroundMeteorController : MonoBehaviour
    {
        public float velocityMaxScale = 2f;
        public float rotationMaxScale = 90f;
        
        public float minXPos = -42f, maxXPos = 42f; 
        public float minYPos = -29f, maxYPos = 29f;

        private Rigidbody2D _rigidbody;
        private Vector2 _moveVector;
        private float _rotation;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }
        private void Start()
        {
            float velocityScale = Random.Range(-velocityMaxScale, velocityMaxScale);
            float velocityAngle = Random.Range(-180f, 180f);

            _rotation = Random.Range(-rotationMaxScale, rotationMaxScale);
            _moveVector = MathUtilities.RotateVector(Vector2.up, velocityAngle) * velocityScale;
        }
        private void Update()
        {
            _rigidbody.linearVelocity = _moveVector;
            _rigidbody.angularVelocity = _rotation;
            ClampMeteorInBorders();
        }
        private void ClampMeteorInBorders()
        {
            float x = transform.position.x;
            float y = transform.position.y;
            if (x < minXPos) transform.position = new Vector2(maxXPos, y);
            else if (x > maxXPos) transform.position = new Vector2(minXPos, y);
            if (y < minYPos) transform.position = new Vector2(x, maxYPos);
            else if (y > maxYPos) transform.position = new Vector2(x, minYPos);
        }
    }
}

