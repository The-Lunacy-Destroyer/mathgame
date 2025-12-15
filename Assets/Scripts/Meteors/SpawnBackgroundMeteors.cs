using UnityEngine;

namespace Meteors
{
    public class SpawnBackgroundMeteors : MonoBehaviour
    {
        public GameObject[] meteorPrefabs;
        public int amount = 50;
        public float minScale = 0.25f, maxScale = 1f;
        
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
                
                float randomScale = Random.Range(minScale, maxScale);
                meteor.transform.localScale *= randomScale;
            }
        }
    }
}
