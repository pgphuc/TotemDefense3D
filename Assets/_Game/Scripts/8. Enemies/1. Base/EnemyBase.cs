using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : GameUnit
{
    #region Serialized Fields
    
    //Composition
    public Component_Health _healthComponent;
    public Component_Attack_Enemy _attackComponent;
    public Component_Move_Enemy _moveComponent;

    
    
    #endregion
    
    #region event implementation

    protected override void OnTargetDeath(GameUnit unit)
    {
        _moveComponent._dualingTarget = null;
        _attackComponent._attackTarget = null;
        base.OnTargetDeath(unit);
    }
    
    #endregion
}
