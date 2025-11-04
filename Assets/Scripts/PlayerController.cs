using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : EntityController
{
    private Camera _mainCamera;
    private Transform _spaceGunTransform;
    
    void Start()
    {
        _mainCamera = Camera.main;
        _spaceGunTransform = transform.Find("SpaceGun");
    }

    void FixedUpdate()
    {
        if (Keyboard.current.cKey.isPressed)
        {
            LaunchProjectile(_spaceGunTransform.position, transform.up);
        }
        MovePlayer();
    }

    private Vector3 GetPlayerToMouseVector()
    {
        Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Mouse.current.position.value);
        return mousePosition - transform.position;
    }

    private void MovePlayer()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Vector2 direction = GetPlayerToMouseVector().normalized;
            Vector2 movement = direction * (Time.fixedDeltaTime * 100f * speed);
            _rigidbody.AddForce(movement);
            _rigidbody.transform.up = direction;
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
