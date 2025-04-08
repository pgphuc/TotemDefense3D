using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : GameUnit
{
    public int goldAmount;
    public Transform _bulletSpawnPoint;

    #region Serialized Fields

    //Composition
    public Component_Health _healthComponent;
    public Component_Attack_Enemy _attackComponent;
    public Component_Move_Enemy _moveComponent;
    public Component_Check_EnemyRanged _checkComponent;
    
    //animator
    public Animator _animator;
    #endregion


    public override void OnDespawn()
    {
        base.OnDespawn();
        EnemyWaveManager.Instance.enemyOnScene.Remove(this);
        _animator.SetTrigger("Dead");
        _moveComponent.StopMoving();
    }

    #region event implementation

    protected override void OnTargetDeath(GameUnit unit)
    {
        _moveComponent._dualingTarget = null;
        _attackComponent._attackTarget = null;
        base.OnTargetDeath(unit);
    }
    
    #endregion
}
