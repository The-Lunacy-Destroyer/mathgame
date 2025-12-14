using Health;
using Projectile;
using UnityEngine;

public class MeteorsController : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 moveDirection;
    float rng1;
    float rng2;
    float force = 10f;

    void Start()
    {
        rng1 = Random.Range(-1.0f, 1f);
        rng2 = Random.Range(-1.0f, 1f);
        rb = GetComponent<Rigidbody2D>();
        moveDirection = new Vector2(rng1, rng2);
    }

    void FixedUpdate()
    {
        rb.position += moveDirection * 0.05f;
        rb.transform.up = Utilities.MathUtilities.RotateVector(rb.transform.up, rng1 + Mathf.Sign(rng1) * 0.5f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        MonoBehaviour otherObj = other.GetComponent<MonoBehaviour>();
        if (otherObj is ProjectileController projectile)
        {
            Rigidbody2D proj_rb = projectile.GetComponent<Rigidbody2D>();
            rb.AddForce(proj_rb.linearVelocity * force);
            Destroy(projectile.gameObject);
        }
        else if (otherObj is EntityHealthController entity)
        {
            entity.CurrentHealth -= rb.linearVelocity.magnitude * rb.mass;
        }
    }
}
