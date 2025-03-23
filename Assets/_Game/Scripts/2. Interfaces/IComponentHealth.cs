using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IComponentHealth
{
    Transform _transform { get; }
    float MaxHealth { get; set; }
    float CurrentHealth { get; set; }
    bool _isActive { get; set; }
    
    bool _isBlocked { get; set; }
    void TakeDamage(float damage);
    
}
