using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : GameUnit
{
    public int goldAmount;

    #region Serialized Fields

    //Composition
    public Component_Health _healthComponent;
    public Component_Attack_Enemy _attackComponent;
    public Component_Move_Enemy _moveComponent;


    #endregion

    public override void OnInit()
    {
        base.OnInit();
        
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
