using UnityEngine;
using UnityEngine.InputSystem;
public class BulletController : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    public float bulletDamage = 2.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.magnitude > 40.0f)
        {
            Destroy(gameObject);
        }
    }
    public void Launch(Vector2 direction, float force)
    {
        _rigidbody.AddForce(direction * force);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        MonoBehaviour otherObj = other.GetComponent<MonoBehaviour>();
        if (otherObj is EnemyController enemy)
        {
            enemy.CurrentHealth -= bulletDamage;
            if (enemy.CurrentHealth <= 0)
            {
                Destroy(enemy.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
