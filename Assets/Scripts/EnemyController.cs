using Mono.Cecil.Cil;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
public class EnemyController : EntityController
{
    [SerializeField] FloatingHealthBar healthBar;

    
    private void Start()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }
    private void Update()
    {
        healthBar.UpdateHealthBar(CurrentHealth, maxHealth);
    }
}
