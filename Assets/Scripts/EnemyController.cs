using Mono.Cecil.Cil;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
public class EnemyController : EntityController
{
    [SerializeField] FloatingHealthBar healthBar;

    [SerializeField] float moveSpeed = 5.0f;
    Rigidbody2D rb;
    Transform target;
    Vector2 moveDirection;

    private void Start()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.Find("Player").transform;
    }
    private void Update()
    {
        healthBar.UpdateHealthBar(CurrentHealth, maxHealth);
        if (target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
            moveDirection = direction;
        }
    }
    private void FixedUpdate()
    {
        if (target)
        {
            rb.linearVelocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
        }
    }
}
