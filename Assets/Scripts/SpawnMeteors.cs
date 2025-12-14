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
            float random_scale = Random.Range(0.5f, 2.0f);

            meteor_rb.mass = Random.Range(0.5f, 2.0f);
            meteor.transform.localScale = new Vector3(random_scale, random_scale, 0);

            Instantiate(meteor, Utilities.MathUtilities.RectRandomPos(-74f, 71f, -46f, 50f), Random.rotation);
        }
    }
}
