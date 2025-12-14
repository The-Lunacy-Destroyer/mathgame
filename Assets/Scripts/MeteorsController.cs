using Health;
using Projectile;
using UnityEngine;

public class MeteorsController : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 moveDirection;
    float rand1;
    float rand2;
    float force = 10f;
    float slowdown = 1f;
    public bool is_background_meteor = false;

    void Start()
    {
        rand1 = Random.Range(-1.0f, 1f);
        rand2 = Random.Range(-1.0f, 1f);
        rb = GetComponent<Rigidbody2D>();
        moveDirection = new Vector2(rand1, rand2);
    }

    void FixedUpdate()
    {
        if (is_background_meteor) slowdown = 0.5f;
        rb.position += moveDirection * 0.05f * slowdown;
        rb.transform.up = Utilities.MathUtilities.RotateVector(rb.transform.up, (rand1 + Mathf.Sign(rand1) * 0.5f) * slowdown);
        float x = rb.position.x;
        float y = rb.position.y;
        if (x < -74 || x > 71 || y < -46 || y > 50)
        {
            float new_x = -1.5f - x;
            float new_y = 2f - y;
            rb.position = new Vector2(new_x, new_y);
        }
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
