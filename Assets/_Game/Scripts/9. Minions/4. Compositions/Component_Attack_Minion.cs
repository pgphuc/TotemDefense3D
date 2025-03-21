using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component_Attack_Minion :ComponentBase, IComponentAttack
{
    public Component_Attack_Minion(GameUnit owner, float damage, float speed)
    {
        _owner = owner;
        _damage = damage;
        _speed = speed;
    }
    public override void OnInit()
    {
        _target = null;
    }
    public GameUnit _owner { get; set; }
    public float _lastAttackTime { get; set; }
    public float _damage { get; set; }
    public float _speed { get; set; }
    public Component_Health _target { get; set; }
    public void MeleeAttack()
    {
        _target.TakeDamage(_damage);
    }
    public void RangedAttack()
    {
        _target.TakeDamage(_damage);
    }
    public void StopAttacking()
    {
        //TODO: cancel attack animation
    }
}
