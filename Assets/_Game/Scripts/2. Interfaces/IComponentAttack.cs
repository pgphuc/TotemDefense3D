using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IComponentAttack
{
    float _lastAttackTime { get; set; }
    float _damage { get; set; }
    float _attackSpeed { get; set; }
    Component_Health _attackTarget { get; set; }
    void StartAttack();
    void StopAttacking();
}
