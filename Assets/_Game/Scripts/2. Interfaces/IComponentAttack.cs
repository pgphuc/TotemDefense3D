using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IComponentAttack
{
    GameUnit _owner { get; set; }
    float _lastAttackTime { get; set; }
    float _damage { get; set; }
    float _speed { get; set; }
    Component_Health _target { get; set; }
    void MeleeAttack();
    void RangedAttack();
    void StopAttacking();
}
