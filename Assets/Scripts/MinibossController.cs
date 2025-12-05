using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MinibossController : EntityController
{
    private Transform Gun1;
    private Transform Gun2;
    private Transform Gun3;
    private Transform Center;
    private Transform target;
    private void Start()
    {
        target = GameObject.Find("Player").transform;
        Gun1 = transform.Find("gun1");
        Gun2 = transform.Find("gun2");
        Gun3 = transform.Find("gun3");
        Center = transform;

    }
    float x = 0;
    float y = 0;
    private void FixedUpdate()
    {
        Vector2 vec1 = (Gun1.position - Center.position).normalized;
        Vector2 vec2 = (Gun2.position - Center.position).normalized;
        Vector2 vec3 = (Gun3.position - Center.position).normalized;
        if (_canLaunchProjectile)
        {

            LaunchProjectile(Gun1.position, vec1, false);
            LaunchProjectile(Gun2.position, vec2, true);
            LaunchProjectile(Gun3.position, vec3, true);
        }


        x += 0.1f;
        y += 0.05f;
        float dx = transform.position.x - target.position.x;
        float dy = transform.position.y - target.position.y;
        float distance = Mathf.Sqrt(dx * dx + dy * dy);
        Debug.Log(distance);
        _rigidbody.transform.up = new Vector3(Mathf.Cos(x), Mathf.Sin(x), 0);
        _rigidbody.position = target.position + new Vector3(Mathf.Sin(y), Mathf.Cos(y)) * distance;

    }
}
