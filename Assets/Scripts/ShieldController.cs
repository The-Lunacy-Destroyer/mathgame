using Projectile;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    private Transform player;
    private Vector2 pos1;
    private Vector2 pos2;
    void Start()
    {
        player = GameObject.Find("Player").transform;
        pos1 = player.position;
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        Debug.Log(rb.transform.up);
        transform.position = player.position + rb.transform.up*2;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        MonoBehaviour otherObj = other.GetComponent<MonoBehaviour>();
        if (otherObj is ProjectileController) Destroy(other.gameObject);

 
    }
}
