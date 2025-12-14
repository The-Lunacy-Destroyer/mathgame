using UnityEngine;

public class SpawnMeteors : MonoBehaviour
{
    public GameObject meteor;
    public int amount = 0;

   void Start()
    {
        for(int i = 0; i < amount; i++)
        {
            float rng1 = Random.Range(-1.0f, 1f);
            float rng2 = Random.Range(-1.0f, 1f);
            float rng3 = Random.Range(3.0f, 100f);
            Vector3 pos = rng3 * new Vector3(rng1, rng2);
            Instantiate(meteor, pos , Quaternion.identity);
        }
    }
}
