using System;
using UnityEngine;

public class EntityController  : MonoBehaviour
{
    public float maxHealth = 100.0f;
    private float _currentHealth;
    public float CurrentHealth
    {
        get => _currentHealth;
        set => _currentHealth = Mathf.Clamp(value, 0, maxHealth);
    }
    
    private void Awake()
    {
        CurrentHealth = maxHealth;
    }
}