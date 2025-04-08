using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Component_Attack_Totem : ComponentBase, IComponentAttack
{
    public Component_Attack_Totem(TotemBase owner, Transform bulletSpawnPoint, float damage, float attackSpeed, float bulletSpeed, float bulletAcceleration)
    {
        _owner = owner;
        _bulletSpawnPoint = bulletSpawnPoint;
        _damage = damage;
        _attackSpeed = attackSpeed;
        _bulletSpeed = bulletSpeed;
        _bulletAcceleration = bulletAcceleration;
    }
    public TotemBase _owner { get; set; }
    public float _lastAttackTime { get; set; }
    public float _damage { get; set; }
    public float _attackSpeed { get; set; }
    public float _bulletSpeed { get; set; }
    public float _bulletAcceleration { get; set; }
    public Component_Health _attackTarget { get; set; }
    public override void OnInit()
    {
        _attackTarget = null;
        GenerateBullet();
    }
    public void Attack()
    {
        _lastAttackTime = Time.time;
        Vector3 start = _bulletSpawnPoint.position;
        Vector3 target = _attackTarget._transform.position + Vector3.down * 1.25f;
        ShootBullet(start, target);
        GenerateBullet();

    }

    public bool FinishCoolDown()
    {
        return Time.time - _lastAttackTime >= _attackSpeed;
    }

    public void StartMeleeAttack()
    {
        _attackTarget = _owner._checkComponent.FindNearestEnemy();
    }

    public void StopAttacking()
    {
        //TODO: cancel attack animation
    }

    public bool EnemyKilledByTotem(Collider target)
    {
        return _attackTarget == ComponentCache.GetHealthComponent(target);
    }
    
    
    #region Bullet implementation
    private TotemBullet _bullet;
    private Transform _bulletSpawnPoint;
    
    private void ShootBullet(Vector3 start, Vector3 end)
    {
        _bullet.Shoot(start, end);
    }
    private void GenerateBullet()
    {
        _bullet = SimplePool.Spawn<TotemBullet>(_owner._bulletPrefab.poolType, _bulletSpawnPoint.position, _owner.transform.rotation);
        _bullet._totemAttackComponent = this;
        _bullet.OnInit();
    }
    #endregion
}
