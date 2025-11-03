using UnityEngine;
using UnityEngine.InputSystem;
public class BulletController : MonoBehaviour
{
    public float bulletDamage = 20.1f;

    public EntityController Source { get; set; }
    
    private Rigidbody2D _rigidbody;
    
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

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
        MonoBehaviour otherObj = other.GetComponent<EntityController>();

        if (otherObj is EnemyController enemy && Source is PlayerController)
        {
            DecreaseHealth(enemy);
        }
        else if (otherObj is PlayerController player && Source is EnemyController)
        {
            DecreaseHealth(player);
        }
    }

    private void DecreaseHealth(EntityController entity)
    {
        entity.CurrentHealth -= bulletDamage;
           
        if (entity.CurrentHealth <= 0)
        {
            Destroy(entity.gameObject);
        }
        Destroy(gameObject);
    }
}
