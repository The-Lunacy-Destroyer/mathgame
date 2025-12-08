using Projectile;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    private Transform player;
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }
    void Update()
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        transform.position = player.position + rb.transform.up*2;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        MonoBehaviour otherObj = other.GetComponent<MonoBehaviour>();
        if (otherObj is ProjectileController projectile && !projectile.SourceObject.CompareTag("Player") ) Destroy(other.gameObject);
    }
}
