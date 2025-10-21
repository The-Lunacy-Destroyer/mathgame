using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb = null;
    private Camera _mainCamera = null;

    public float speed = 1f;
    public float maxSpeed = 10f;
    public float slowdown = 5f;
    public float projectileSpeed = 100f;
    public GameObject projectilePrefab;

    Vector2 projectile_direction;
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
        LaunchProjectile();
    }

    private Vector3 GetPlayerToMouseVector()
    {
        Vector3 mousepos = _mainCamera.ScreenToWorldPoint(Mouse.current.position.value);
        return mousepos - transform.position;
    }

    private void MovePlayer()
    {
        Vector2 direction = new Vector2(1, 0);
        if (Mouse.current.leftButton.isPressed)
        {
            direction = GetPlayerToMouseVector().normalized;
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

    private void LaunchProjectile()
    {
        if (Keyboard.current.cKey.isPressed)
        {
            GameObject projectileObject = Instantiate(projectilePrefab, _rb.position, Quaternion.identity);
            BulletController projectile = projectileObject.GetComponent<BulletController>();
            projectile.Launch(_rb.transform.up, projectileSpeed);
        }
    }
}
