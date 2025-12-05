using Health;
using UnityEngine;

namespace Drop
{
    public class HealthDropController : MonoBehaviour
    {
        public float healValue = 10f;
    
        private void OnTriggerEnter2D(Collider2D collision)
        {
            EntityHealthController entityHealth = 
                collision.gameObject.GetComponent<EntityHealthController>();
            if (entityHealth)
            {
                entityHealth.CurrentHealth += healValue;
                Destroy(gameObject);
            }
        }
    }
}
