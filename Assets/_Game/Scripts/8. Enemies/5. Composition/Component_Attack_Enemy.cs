using UnityEngine;

public class Component_Attack_Enemy : ComponentBase,  IComponentAttack
{
    public Component_Attack_Enemy(EnemyBase owner, float damage, float attackSpeed)
    {
        _owner = owner;
        _damage = damage;
        _attackSpeed = attackSpeed;
    }
    public EnemyBase _owner { get; set; }
    public float _lastAttackTime { get; set; }
    public float _damage { get; set; }
    public float _attackSpeed { get; set; }
    
    public Component_Health _attackTarget { get; set; }
    
    
    public override void OnInit()
    {
        _attackTarget = null;
    }

    public void StartAttack()
    {
        _lastAttackTime = Time.time - _attackSpeed;
        _attackTarget = _owner._moveComponent._dualingTarget;
    }

    public bool FinishCooldown()
    {
        return Time.time - _lastAttackTime >= _attackSpeed;
    }
    public void MeleeAttack()
    {
        _lastAttackTime = Time.time;
        _attackTarget.TakeDamage(_damage);
        _owner._animator.SetTrigger("Attack");
    }
    public void StopAttacking()
    {
        //TODO: cancel attack animation
    }

    public bool IsAttackingVillage()
    {
        return _attackTarget?._owner is VillageBase;
    }

    
    
    public void RangedAttack()
    {
        _lastAttackTime = Time.time;
        _attackTarget.TakeDamage(_damage);
        //TODO: SetTrigger attack range
    }
    
    public bool EnemyKilledByOwner(Collider target)
    {
        return _attackTarget == ComponentCache.GetHealthComponent(target);
    }
    #region Bullet implementation
    private BulletBase _bullet;
    private Transform _bulletSpawnPoint;
    
    private void ShootBullet(Vector3 start, Vector3 end)
    {
        _bullet.Shoot(start, end);
    }
    private void GenerateBullet()
    {
        EnemyRangedBase enemy = (EnemyRangedBase)_owner;
        _bullet = SimplePool.Spawn<BulletBase>(enemy._bulletPrefab.poolType, _bulletSpawnPoint.position, _owner.transform.rotation);
        // _bullet._totemAttackComponent = this;
        _bullet.OnInit();
    }
    #endregion
}
