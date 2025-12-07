using UnityEngine;

namespace Movement
{
    public class EntityMaxSpeedRandomizer : MonoBehaviour
    {
        public float minSpeedScale = 0.75f;
        public float maxSpeedScale = 1.25f;
        
        private void Start()
        {
            EntityController entity = GetComponent<EntityController>();
            if (entity && entity is IEntityMovable movable)
            {
                movable.MaxSpeed *= Random.Range(minSpeedScale, maxSpeedScale);
            }
        }
    }
}