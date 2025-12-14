using Health;
using Projectile;
using UnityEngine;

public class MeteorsController : EntityController
{
    Rigidbody2D rb;
    float rand1;
    float rand2;
    float slowdown = 1f;

    void Start()
    {
        rand1 = Random.Range(-1.0f, 1f);
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.transform.up = Utilities.MathUtilities.RotateVector(rb.transform.up, (rand1 + Mathf.Sign(rand1) * 0.5f) * slowdown);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        MonoBehaviour otherObj = other.GetComponent<MonoBehaviour>();
        if (otherObj is ProjectileController projectile)
        {
            Rigidbody2D proj_rb = projectile.GetComponent<Rigidbody2D>();
            Destroy(projectile.gameObject);
        }
        else if (otherObj is EntityHealthController entity)
        {
            entity.CurrentHealth -= rb.linearVelocity.magnitude * rb.mass;
        }
    }
}
