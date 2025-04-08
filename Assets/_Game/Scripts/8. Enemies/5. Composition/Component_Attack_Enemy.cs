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
        if (_owner is EnemyRangedBase)
        {
            _bulletSpawnPoint = _owner._bulletSpawnPoint;
            GenerateBullet();
        }
    }

    public void StartMeleeAttack()
    {
        _lastAttackTime = Time.time - _attackSpeed;
        _attackTarget = _owner._moveComponent._dualingTarget;
        _owner._moveComponent._dualingTarget = null;
        
        RotateOwnerToTarget();
    }

    public void StartRangeAttack()
    {
        _lastAttackTime = Time.time - _attackSpeed;
        _attackTarget = _owner._checkComponent.FindNearestEnemy();
        _owner._moveComponent._dualingTarget = null;
        
        RotateOwnerToTarget();
    }
    private void RotateOwnerToTarget()
    {
        float rotateSpeed = 100f; // độ xoay mỗi giây
        Vector3 direction = _attackTarget._transform.position - _owner.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        _owner.transform.rotation = Quaternion.RotateTowards(_owner.transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
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

    public bool IsAttackingStructure()
    {
        return _attackTarget?._owner is VillageBase or TotemBase;
    }

    public void BombAttack()
    {
        _owner._animator.SetBool("isWalking", false);
        
        PlayerInteraction.Instance.UnSubcribeEnemyOnDeath(_owner);
        
        _owner._animator.SetTrigger("Attack");
        float damage = 1000f;
        _attackTarget.TakeDamage(damage);
        _owner._healthComponent.TakeDamage(damage);
    }
    
    public void RangedAttack()
    {
        _lastAttackTime = Time.time;
        Vector3 start = _bulletSpawnPoint.position;
        Vector3 target = _attackTarget._transform.position + Vector3.down * 1.25f;
        ShootBullet(start, target);
        GenerateBullet();
        //TODO: SetTrigger attack range
    }
    
    public bool EnemyKilledByOwner(Collider target)
    {
        return _attackTarget == ComponentCache.GetHealthComponent(target);
    }
    
    
    #region Bullet implementation
    private EnemyBullet _bullet;
    public Transform _bulletSpawnPoint;//FIXME: when come to animation
    
    private void ShootBullet(Vector3 start, Vector3 end)
    {
        _bullet.Shoot(start, end);
    }
    private void GenerateBullet()
    {
        EnemyRangedBase enemy = (EnemyRangedBase)_owner;
        _bullet = SimplePool.Spawn<EnemyBullet>(enemy._bulletPrefab.poolType, _bulletSpawnPoint.position, _owner.transform.rotation);
        _bullet.transform.SetParent(_owner.transform);
        _bullet.OnInit();
    }
    #endregion
}
