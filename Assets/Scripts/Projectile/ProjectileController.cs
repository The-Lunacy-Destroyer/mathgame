using System;
using System.Collections;
using Enemies;
using Health;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace Projectile
{
    public class ProjectileController : MonoBehaviour
    {
        public GameObject flashPrefab;
        public float damage = 20.1f;
        public GameObject SourceObject { get; set; }

        protected void InstantiateFlash()
        {
            GameObject flash = Instantiate(flashPrefab, transform.position, Quaternion.identity);
            flash.transform.up = -transform.up;
        }
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
                    InstantiateFlash();
                    DecreaseHealth(otherEntityHealth);
                }
            }
        }

        private void DecreaseHealth(EntityHealthController entityHealth)
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
