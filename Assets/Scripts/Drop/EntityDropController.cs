using System;
using Health;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Drop
{
    public class EntityDropController : MonoBehaviour
    {
        private EntityHealthController _healthSystem;
        
        [Serializable]
        public struct Drop
        {
            public GameObject dropObj;
            [Range(0, 1)] public float dropChance;
        }
        
        public Drop[] drops;
        public bool isSingleDrop = true;

        private void Start()
        {
            _healthSystem = GetComponent<EntityHealthController>();
        }
        
        private void OnDestroy()
        {
            if(!(_healthSystem && _healthSystem.CurrentHealth <= 0)) return;
            
            foreach (Drop drop in drops)
            {
                if (Random.value <= drop.dropChance)
                {
                    Instantiate(drop.dropObj, transform.position, Quaternion.identity);
                    if (isSingleDrop) return;
                }
            }
        }
    }
}