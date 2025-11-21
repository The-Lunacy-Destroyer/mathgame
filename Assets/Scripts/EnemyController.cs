using System;
using Mono.Cecil.Cil;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
public class EnemyController : EntityController
{
    private Transform _targetTransform;
    
    public float slowdownRadius = 4f;
    public float shootRadius = 3f;

    private Vector2 _movementVector;
    private Vector2 _movementDirection;

    public GameObject healthDrop;
    
    private void Start()
    {
        _targetTransform = GameObject.Find("Player").transform;
        _movementVector = _targetTransform.position - transform.position;
        _movementDirection = _movementVector.normalized;
    }
    
    void FixedUpdate()
    {
        if (_targetTransform)
        {
            _movementVector = _targetTransform.position - transform.position;
            _movementDirection = _movementVector.normalized;

            if (_movementVector.magnitude <= shootRadius)
            {
                LaunchProjectile(_rigidbody.position, _movementDirection);
            }
            
            MoveEnemy();
        }
    }

    private void OnDestroy()
    {
        if(CurrentHealth <= 0)
            Instantiate(healthDrop, transform.position, Quaternion.identity);
    }

    private void MoveEnemy()
    {
        float angle = Mathf.Atan2(_movementDirection.x, _movementDirection.y) * Mathf.Rad2Deg;
        _rigidbody.rotation = angle;
        
        float forceAngle = Mathf.Atan2(_rigidbody.totalForce.x, _rigidbody.totalForce.y) * Mathf.Rad2Deg;
        
        if (_movementVector.magnitude > slowdownRadius)
        {
            float forceSpeed = speed * (1 + Mathf.Abs(angle - forceAngle) / 360f);
            _rigidbody.AddForce(_movementDirection * forceSpeed);
        }
        else if (_rigidbody.linearVelocity.magnitude >= slowdown)
        {
            _rigidbody.linearVelocity -= _rigidbody.linearVelocity.normalized * slowdown;
        }
        else
        {
            _rigidbody.linearVelocity = Vector2.zero;
        }

        _rigidbody.linearVelocity = Vector2.ClampMagnitude(_rigidbody.linearVelocity, maxSpeed);
    }
}
