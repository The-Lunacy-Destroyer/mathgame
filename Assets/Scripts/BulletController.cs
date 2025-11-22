using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    
    public float bulletDamage = 20.1f;

    private Camera _camera;
    
    public EntityController Source { get; set; }
    
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
    }

    void Update()
    {
        if ((_camera.transform.position - transform.position).magnitude > 40.0f)
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
