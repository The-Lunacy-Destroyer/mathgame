using UnityEngine;

namespace Meteors
{
    public class SpawnMeteors : MonoBehaviour
    {
        public GameObject[] meteorPrefabs;
        public int amount = 20;
        public float minScale = 0.5f, maxScale = 3f;
        public float minMass = 0.5f, maxMass = 5f;
        
        public float minXPos = -64f, maxXPos = 64f; 
        public float minYPos = -64f, maxYPos = 64f;

        void Start()
        {
            for(int i = 0; i < amount; i++)
            {
                GameObject meteorPrefab = meteorPrefabs[Random.Range(0, meteorPrefabs.Length)];
            
                GameObject meteor = Instantiate(
                    meteorPrefab, 
                    Utilities.MathUtilities.RandomRectPosition(minXPos, maxXPos, minYPos, maxYPos), 
                    Quaternion.identity,
                    transform
                );
                
                Rigidbody2D meteorRb = meteor.GetComponent<Rigidbody2D>();
                SpriteRenderer meteorSr = meteor.GetComponent<SpriteRenderer>();
                
                float randomScale = Random.Range(minScale, maxScale);
                float randomMass = Random.Range(minMass, maxMass);

                float colorScale = (randomMass - 0.5f) / 4.5f;

                meteorRb.mass = randomMass;

                meteorSr.color = new Color(
                    0.6f + colorScale * 0.4f, 
                    0.4f + colorScale * 0.6f, 
                    0.4f + colorScale * 0.6f
                );
                
                meteor.transform.localScale *= randomScale;
            }
        }
    }
}
