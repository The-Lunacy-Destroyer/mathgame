using Health;
using Projectile;
using UnityEngine;

public class BackGroundMeteors : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 moveDirection;
    float rand1;
    float rand2;
    float slowdown = 1f;

    void Start()
    {
        rand1 = Random.Range(-1.0f, 1f);
        rand2 = Random.Range(-1.0f, 1f);
        rb = GetComponent<Rigidbody2D>();

        slowdown = 0.5f;
        moveDirection = new Vector2(rand1, rand2);
    }

    void FixedUpdate()
    {
        rb.position += moveDirection * 0.05f * slowdown;
        rb.transform.up = Utilities.MathUtilities.RotateVector(rb.transform.up, (rand1 + Mathf.Sign(rand1) * 0.5f) * slowdown);
        float x = rb.position.x;
        float y = rb.position.y;
        if (x < -74 || x > 71 || y < -46 || y > 50)
        {
            float new_x = -1.5f - x * 0.98f;
            float new_y = 2f - y * 0.98f;
            rb.position = new Vector2(new_x, new_y);
        }
    }
}

