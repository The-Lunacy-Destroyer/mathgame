using Projectile;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    private Transform _owner;
    private Rigidbody2D _rb;

    public float shieldOffset = 1.5f;
    
    void Start()
    {
        _owner = GameObject.Find("Player").transform;
        _rb = _owner.GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        transform.position = _owner.position + _rb.transform.up * shieldOffset;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        MonoBehaviour otherObj = other?.GetComponent<MonoBehaviour>();
        if (otherObj is ProjectileController projectile
            && other.CompareTag("Bullet")
            && !projectile.SourceObject.CompareTag("Player"))
        {
            Destroy(other.gameObject);
        }
    }
}
