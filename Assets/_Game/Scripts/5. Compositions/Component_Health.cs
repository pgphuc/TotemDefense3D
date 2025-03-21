using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component_Health : ComponentBase, IComponentHealth
{
    public Component_Health(GameUnit owner, Transform transform, float maxHealth)
    {
        _owner = owner;
        _transform = transform;
        MaxHealth = maxHealth;
    }
    public override void OnInit()
    {
        CurrentHealth = MaxHealth;
    }

    private GameUnit _owner { get; set; }//object giữ component
    public Transform _transform { get; }
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public bool _isActive { get; set; } = true;
    
    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            _isActive = false;
            _owner.OnDespawn();
        }
    }
    
}
