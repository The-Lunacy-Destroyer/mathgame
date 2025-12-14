using UnityEngine;
public class BackgroundMeteorsSpawn : MonoBehaviour
{
    public GameObject meteor1;
    public GameObject meteor2;
    public int amount = 0;

    void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            bool random_bool = Random.value > 0.5f ? true : false;
            float random_scale = Random.Range(0.4f, 1f);

            meteor1.transform.localScale = new Vector3(random_scale, random_scale, 0);
            meteor2.transform.localScale = new Vector3(random_scale, random_scale, 0);

            Instantiate(random_bool ? meteor1 : meteor2, Utilities.MathUtilities.RectRandomPos(-74f, 71f, -46f, 50f), Random.rotation);
        }
    }
}
