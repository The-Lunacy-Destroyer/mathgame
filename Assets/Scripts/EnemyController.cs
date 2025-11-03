using Mono.Cecil.Cil;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
public class EnemyController : EntityController
{
    private Transform _targetTransform;
    
    public float movementSpeed = 5.0f;
    private Vector2 _movementDirection;
    
    private void Start()
    {
        _targetTransform = GameObject.Find("Player").transform;
    }
    protected override void Update()
    {
        base.Update();
        if (_targetTransform)
        {
            _movementDirection = (_targetTransform.position - transform.position).normalized;
            _rigidbody.linearVelocity = _movementDirection * movementSpeed;
            
            float angle = Mathf.Atan2(_movementDirection.x, _movementDirection.y) * Mathf.Rad2Deg;
            _rigidbody.rotation = angle;
        }
    }

    void FixedUpdate()
    {
        LaunchProjectile(_movementDirection);
    }
}
