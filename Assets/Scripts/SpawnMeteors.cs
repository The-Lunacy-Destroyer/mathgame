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
            SpriteRenderer meteor_sr = meteor.GetComponent<SpriteRenderer>();
            float random_scale = Random.Range(0.5f, 2.0f);
            float random_mass = Random.Range(0.5f, 5.0f);

            float v = (random_mass - 0.5f) / 4.5f;


            meteor_rb.mass = random_mass;
            meteor_sr.color = new Color(0.6f + v * 0.4f, 0.4f + v * 0.6f, 0.4f + v * 0.6f);
            meteor.transform.localScale = new Vector3(random_scale, random_scale, 0);

            Instantiate(meteor, Utilities.MathUtilities.RectRandomPos(-74f, 71f, -46f, 50f), Random.rotation);
        }
    }
}
