using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component_Attack_Minion :ComponentBase, IComponentAttack
{
    public Component_Attack_Minion(MinionBase owner, float damage, float speed)
    {
        _owner = owner;
        _damage = damage;
        _speed = speed;
    }
    public override void OnInit()
    {
        _attackTarget = null;
    }
    public MinionBase _owner { get; set; }
    public Component_Health _attackTarget { get; set; }
    public float _lastAttackTime { get; set; }
    public float _damage { get; set; }
    public float _speed { get; set; }

    public void StartAttack()
    {
        _attackTarget = _owner._moveComponent._dualingTarget;
        _lastAttackTime = Time.time;
        if (_attackTarget._isBlocked)
        {
            _owner._checkComponent._isFindingUnblockedEnemy = true;
        }
        else
        {
            _owner._healthComponent._isBlocked = true;
            _attackTarget._isBlocked = true;
            _owner._checkComponent._isFindingUnblockedEnemy = false;
        }
    }
    public void MeleeAttack()
    {
        _lastAttackTime = Time.time;
        _attackTarget.TakeDamage(_damage);
    }
    public void RangedAttack()
    {
        _attackTarget.TakeDamage(_damage);
    }
    public void StopAttacking()
    {
        //TODO: cancel attack animation
    }

    public bool FinishCooldown()
    {
        return Time.time - _lastAttackTime >= _speed;
    }
}
