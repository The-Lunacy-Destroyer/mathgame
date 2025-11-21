using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : EntityController
{
    private Camera _mainCamera;
    private Transform _spaceGunTransform;

    private GameObject _ingameMenu;

    public float rotationSpeed = 5f;
    public float decelerationForce = 1f;
    void Start()
    {
        _mainCamera = Camera.main;
        _spaceGunTransform = transform.Find("SpaceGun");
        _ingameMenu = GameObject.Find("IngameMenu");
        _ingameMenu.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
        RotateGun();
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            _ingameMenu.SetActive(!_ingameMenu.activeSelf);
        }
    }

    void FixedUpdate()
    {
        if (Keyboard.current.cKey.isPressed || Mouse.current.rightButton.isPressed)
        {
            LaunchProjectile(_spaceGunTransform.position, transform.up);
        }
        MovePlayer();
    }

    void OnDestroy()
    {
        if (_ingameMenu && !_ingameMenu.activeSelf)
        {
            _ingameMenu.SetActive(true);
        }
    }
    
    private Vector3 GetPlayerToMouseVector()
    {
        Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Mouse.current.position.value);
        return mousePosition - transform.position;
    }

    private void MovePlayer()
    {
        Vector2 movementDirection = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) movementDirection.y++;
        if (Keyboard.current.sKey.isPressed) movementDirection.y--;
        if (Keyboard.current.dKey.isPressed) movementDirection.x++;
        if (Keyboard.current.aKey.isPressed) movementDirection.x--;
        
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
        if (!Mouse.current.leftButton.isPressed) return;
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
