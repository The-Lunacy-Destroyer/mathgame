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
        Debug.Log(Gun1.rotation);
        if (_canLaunchProjectile)
        {
            LaunchProjectile(Gun1.position, new Vector2(0, 1), false);
            LaunchProjectile(Gun2.position, new Vector2(1, -1), true);
            LaunchProjectile(Gun3.position, new Vector2(-1, -1), true);
        }
        
    }
}
