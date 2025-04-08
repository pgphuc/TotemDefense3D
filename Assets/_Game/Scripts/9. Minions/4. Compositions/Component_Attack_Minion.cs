using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component_Attack_Minion :ComponentBase, IComponentAttack
{
    public Component_Attack_Minion(MinionBase owner, float damage, float attackSpeed)
    {
        _owner = owner;
        _damage = damage;
        _attackSpeed = attackSpeed;
    }
    public override void OnInit()
    {
        _attackTarget = null;
    }
    public MinionBase _owner { get; set; }
    public Component_Health _attackTarget { get; set; }
    public float _lastAttackTime { get; set; }
    public float _damage { get; set; }
    public float _attackSpeed { get; set; }
    

    public void StartMeleeAttack()
    {
        _attackTarget = _owner._moveComponent._dualingTarget;
        _owner._moveComponent._dualingTarget = null;
        
        RotateOwnerToTarget();
        
        _lastAttackTime = Time.time - _attackSpeed;
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

    private void RotateOwnerToTarget()
    {
        float rotateSpeed = 100f; // độ xoay mỗi giây
        Vector3 direction = _attackTarget._transform.position - _owner.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        _owner.transform.rotation = Quaternion.RotateTowards(_owner.transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }
    public void MeleeAttack()
    {
        _lastAttackTime = Time.time;
        _attackTarget.TakeDamage(_damage);
        _owner._animator.SetTrigger("Attack");
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
        return Time.time - _lastAttackTime >= _attackSpeed;
    }
}
