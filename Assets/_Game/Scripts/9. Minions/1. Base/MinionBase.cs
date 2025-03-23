using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MinionBase : GameUnit
{
    public VillageBase villageSpawner;
    public BarrackBase barrackSpawner;
    
    
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
    
    #endregion
    
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
    
    public override void OnDespawn()
    {
        base.OnDespawn();
        switch (movingType)
        {
            case MovingType.Reinforcing:
            case MovingType.ReachedVillage:
                villageSpawner.reinforcementSpawned--;
                break;
            case MovingType.Defending:
            case MovingType.AfterMatch:
                switch (minionType)
                {
                    case MinionType.Village:
                        villageSpawner.defenseCount--;
                        villageSpawner.defenseSpawned--;
                        break;
                    case MinionType.Barrack:
                        barrackSpawner._minionCapacity++;
                        break;
                }
                break;
            case MovingType.ReachedBarrack:
                switch (minionType)
                {
                    case MinionType.Village:
                        villageSpawner.reinforcementSpawned--;
                        break;
                    case MinionType.Barrack:
                        barrackSpawner.defenseSpawned--;
                        break;
                }
                break;
        }
        
    }
    
    
}
