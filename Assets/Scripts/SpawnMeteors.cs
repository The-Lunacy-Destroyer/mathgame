using UnityEngine;

public class SpawnMeteors : MonoBehaviour
{
    public GameObject meteor;
    public int amount = 0;

   void Start()
    {
        for(int i = 0; i < amount; i++)
        {
            Rigidbody2D meteor_rb = meteor.GetComponent<Rigidbody2D>();
            float rng = Random.Range(0.5f, 2.0f);

            meteor_rb.mass = Random.Range(0.5f, 2.0f);
            meteor.transform.localScale = new Vector3(rng, rng, 0);
            Vector3 pos = Random.Range(3.0f, 100f) * Random.insideUnitCircle;

            Instantiate(meteor, pos , Quaternion.identity);
        }
    }
}
