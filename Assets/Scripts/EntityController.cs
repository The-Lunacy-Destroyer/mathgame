using System;
using UnityEngine;

public class EntityController  : MonoBehaviour
{
    public float maxHealth = 100.0f;
    private float _currentHealth = 0.0f;
    public float CurrentHealth
    {
        get => _currentHealth;
        set => _currentHealth = Mathf.Clamp(value, 0, maxHealth);
    }

    private void Start()
    {
        CurrentHealth = maxHealth;
    }
}