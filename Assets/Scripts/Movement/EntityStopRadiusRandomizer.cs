using UnityEngine;

namespace Movement
{
    public class EntityStopRadiusRandomizer : MonoBehaviour
    {
        public float minStopRadiusScale = 0.75f;
        public float maxStopRadiusScale = 1.25f;
        
        private void Start()
        {
            EntityController entity = GetComponent<EntityController>();
            if (entity && entity is IEntityRadiusStoppable stoppable)
            {
                stoppable.StopRadius *= Random.Range(minStopRadiusScale, maxStopRadiusScale);
            }
        }
    }
}