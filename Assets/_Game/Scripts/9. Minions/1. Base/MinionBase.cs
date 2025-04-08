using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MinionBase : GameUnit
{
    public Component_Spawner_Base _spawner;
    #region enum variables
    public MinionType minionType;
    public MovingType movingType;
    #endregion
    
    
    #region Serialized Fields
    
    //Composition
    public Component_Health _healthComponent;
    public Component_Attack_Minion _attackComponent;
    public Component_Move_Minion _moveComponent;
    public Component_Check_Minion _checkComponent;
    
    public Animator _animator;
    
    #endregion

   
    public override void OnDespawn()
    {
        base.OnDespawn();
        if (movingType is MovingType.ReachedBarrack or MovingType.ReachedVillage)
            return;
        _animator.SetTrigger("Dead");
        _moveComponent.StopMoving();
    }
    
    
    #region event implementation

    protected override void OnTargetDeath(GameUnit unit)
    {
        _moveComponent._dualingTarget = null;
        _attackComponent._attackTarget = null;
        _checkComponent._targetsInRange.Remove(unit.gameObject?.GetComponent<Collider>());
        _checkComponent._isFindingUnblockedEnemy = false;
        base.OnTargetDeath(unit);
    }
    #endregion
}
