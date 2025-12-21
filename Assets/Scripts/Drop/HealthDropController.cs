using Health;
using UnityEngine;

namespace Drop
{
    public class HealthDropController : MonoBehaviour
    {
        public float healValue = 10f;
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            EntityHealthController otherEntityHealth = 
                other.GetComponent<EntityHealthController>();
            if (otherEntityHealth && other.CompareTag("Player"))
            {
                otherEntityHealth.CurrentHealth += healValue;
                Destroy(gameObject);
            }
        }
    }
}
