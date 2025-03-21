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

    public override void OnDespawn()
    {
        if (MapManager.Instance.surroundBasePoints.TryGetValue(_moveComponent._target, out bool isOccupied))
        {
            if (isOccupied)
            {
                MapManager.Instance.surroundBasePoints[_moveComponent._target] = false;
            }
        }
        base.OnDespawn();
    }
}
