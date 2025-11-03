using Mono.Cecil.Cil;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
public class EnemyController : EntityController
{
    [SerializeField] FloatingHealthBar healthBar;

    [SerializeField] float moveSpeed = 5.0f;
    Transform target;

    private void Start()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        target = GameObject.Find("Player").transform;
    }
    protected override void Update()
    {
        base.Update();
        healthBar.UpdateHealthBar(CurrentHealth, maxHealth);
        if (target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _rigidbody.rotation = angle;
            moveDirection = direction;
        }
    }

    void FixedUpdate()
    {
        LaunchProjectile(moveDirection);
        if (target)
        {
            _rigidbody.linearVelocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
        }
    }
}
