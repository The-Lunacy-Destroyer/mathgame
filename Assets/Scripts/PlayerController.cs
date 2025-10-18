using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb = null;
    private Camera _mainCamera = null;

    public float speed = 1f;
    public float maxSpeed = 10f;
    public float slowdown = 5f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private Vector3 GetPlayerToMouseVector()
    {
        Vector3 mousepos = _mainCamera.ScreenToWorldPoint(Mouse.current.position.value);
        return mousepos - transform.position;
    }

    private void MovePlayer()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Vector2 direction = GetPlayerToMouseVector().normalized;
            Vector2 movement = direction * (Time.fixedDeltaTime * 100f * speed);
            _rb.AddForce(movement);
            _rb.transform.up = direction;
        }
        else if (_rb.linearVelocity.magnitude > 0)
        {
            _rb.AddForce(-_rb.linearVelocity.normalized * slowdown);
        }
        if (_rb.linearVelocity.magnitude > maxSpeed)
        {
            _rb.linearVelocity = _rb.linearVelocity.normalized * maxSpeed;
        }
    }
}
