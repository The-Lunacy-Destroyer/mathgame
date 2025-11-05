using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : EntityController
{
    private Camera _mainCamera;
    private Transform _spaceGunTransform;
    public InputAction moveVector;

    public float rotationSpeed = 5f;
    public float decelerationForce = 1f;
    void Start()
    {
        moveVector.Enable();
        _mainCamera = Camera.main;
        _spaceGunTransform = transform.Find("SpaceGun");
    }

    protected override void Update()
    {
        base.Update();
        RotateGun();
    }

    void FixedUpdate()
    {
        if (Keyboard.current.cKey.isPressed || Mouse.current.leftButton.isPressed)
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
        Vector2 movementDirection = moveVector.ReadValue<Vector2>().normalized;

        if (movementDirection.magnitude > 0)
        {
            float deceleration = Vector2.Angle(_rigidbody.totalForce, movementDirection) / 180f * decelerationForce;
            float forceSpeed = speed * (1 + deceleration);
            _rigidbody.AddForce(movementDirection * forceSpeed);
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

    private void RotateGun()
    {
        Vector2 mouseVector = GetPlayerToMouseVector();
        Vector2 direction = mouseVector.normalized;
        
        // _rigidbody.transform.up = direction;
        _rigidbody.transform.up = Vector2.MoveTowards(
            _rigidbody.transform.up,
            direction,
            rotationSpeed * Time.deltaTime
        );
    }
}
