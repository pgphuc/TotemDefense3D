using UnityEngine;

public class Component_Attack_Enemy : ComponentBase,  IComponentAttack
{
    public Component_Attack_Enemy(EnemyBase owner, float damage, float speed)
    {
        _owner = owner;
        _damage = damage;
        _speed = speed;
    }
    public EnemyBase _owner { get; set; }
    public float _lastAttackTime { get; set; }
    public float _damage { get; set; }
    public float _speed { get; set; }
    public Component_Health _attackTarget { get; set; }
    public override void OnInit()
    {
        _attackTarget = null;
    }

    public void StartAttack()
    {
        _lastAttackTime = Time.time;
        _attackTarget = _owner._moveComponent._dualingTarget;
    }

    public bool FinishCooldown()
    {
        return Time.time - _lastAttackTime >= _speed;
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

    public bool IsAttackingVillage()
    {
        return _attackTarget._owner is VillageBase;
    }
}
