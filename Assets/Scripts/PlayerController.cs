using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : EntityController
{
    private Camera _mainCamera;

    public float speed = 1f;
    public float maxSpeed = 10f;
    public float slowdown = 5f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    protected override void Update() 
    {
        base.Update();
    }

    void FixedUpdate()
    {
        if (Keyboard.current.cKey.isPressed)
        {
            LaunchProjectile(transform.up);
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
        else if (_rigidbody.linearVelocity.magnitude > 0)
        {
            _rigidbody.AddForce(-_rigidbody.linearVelocity.normalized * slowdown);
        }
        if (_rigidbody.linearVelocity.magnitude > maxSpeed)
        {
            _rigidbody.linearVelocity = _rigidbody.linearVelocity.normalized * maxSpeed;
        }
    }
}
