using System;
using UnityEngine;

public class EntityController  : MonoBehaviour
{
    public float maxHealth = 100.0f;
    private float _currentHealth;

    public float projectileCooldown = 0.5f;
    public float _launchTimer;
    public bool _canLaunchProjectile = true;

    public float CurrentHealth
    {
        get => _currentHealth;
        set => _currentHealth = Mathf.Clamp(value, 0, maxHealth);
    }
    
     void Awake()
    {
        CurrentHealth = maxHealth;
        _launchTimer = projectileCooldown;
    }

    void Update()
    {
        
    }
}