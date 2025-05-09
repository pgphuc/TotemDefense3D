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
        _isActive = true;
        _isBlocked = false;
    }

    public GameUnit _owner { get;  }//object giữ component
    public Transform _transform { get; }
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public bool _isActive { get; set; }
    
    public bool _isBlocked { get; set; }
    
    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        
        if (_owner is VillageBase)
        {
            SoundManager.Instance.PlaySoundOneShot(SoundManager.Instance.villageDamaged);
        }
        
        _owner.InvokeHealthChanged();
        if (CurrentHealth <= 0 && _isActive)
        {
            _isActive = false;
            _owner.OnDespawn();
        }
    }

}
