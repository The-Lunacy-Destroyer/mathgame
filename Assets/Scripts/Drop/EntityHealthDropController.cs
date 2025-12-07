using Health;
using UnityEngine;

namespace Drop
{
    public class EntityHealthDropController : MonoBehaviour
    {
        private EntityHealthController _healthSystem;
        
        public GameObject healthDrop;

        private void Start()
        {
            _healthSystem = GetComponent<EntityHealthController>();
        }
        
        private void OnDestroy()
        {
            if(_healthSystem && _healthSystem.CurrentHealth <= 0)
                Instantiate(healthDrop, transform.position, Quaternion.identity);
        }
    }
}