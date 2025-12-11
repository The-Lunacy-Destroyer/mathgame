using Enemies;
using Health;
using Unity.VisualScripting;
using UnityEngine;

namespace Projectile
{
    public class ProjectileController : MonoBehaviour
    {
        public float damage = 20.1f;
        public GameObject SourceObject { get; set; }

        protected virtual void OnTriggerStay2D(Collider2D other)
        {
            EntityHealthController otherEntityHealth = 
                other.GetComponent<EntityHealthController>();
            
            if (SourceObject && otherEntityHealth)
            {
                if (other.CompareTag("Enemy") && SourceObject.CompareTag("Player")
                    || 
                    other.CompareTag("Player") && SourceObject.CompareTag("Enemy"))
                {
                    DecreaseHealth(otherEntityHealth);
                }
            }
        }

        protected virtual void DecreaseHealth(EntityHealthController entityHealth)
        {
            entityHealth.CurrentHealth -= damage;
           
            if (entityHealth.CurrentHealth <= 0)
            {
               
                if (SourceObject.CompareTag("Player"))
                {
                    
                    PlayerController player = SourceObject.GetComponent<PlayerController>();
                    string enemyName = entityHealth.name.Substring(0, 6);

                    switch (enemyName)
                    {
                        case "Enemy1":
                            player.score += 30;
                            break;
                        case "Enemy2":
                            player.score += 10;
                            break;
                        case "Minibo":
                            player.score += 1000;
                            break;
                    }
                    player.enemyKillCounter++;

                }
                Destroy(entityHealth.gameObject);
            }
        }
    }
}
