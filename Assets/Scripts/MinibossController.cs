using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MinibossController : EntityController
{
    private Transform Gun1;
    private Transform Gun2;
    private Transform Gun3;
    private Transform Center;
    private void Start()
    {
        
        Gun1 = transform.Find("gun1");
        Gun2 = transform.Find("gun2");
        Gun3 = transform.Find("gun3");
        Center = transform.Find("Circle");
    }
    private void FixedUpdate()
    {
        Vector2 vec1 = (Gun1.position - Center.position).normalized;
        Vector2 vec2 = (Gun2.position - Center.position).normalized;
        Vector2 vec3 = (Gun3.position - Center.position).normalized;
        if (_canLaunchProjectile) { 
        
            LaunchProjectile(Gun1.position, vec1, false);
            LaunchProjectile(Gun2.position, vec2, true);
            LaunchProjectile(Gun3.position, vec3, true);
        }
        
    }
}
