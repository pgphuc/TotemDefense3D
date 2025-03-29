using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Component_Spawner_Base : ComponentBase
{
    
    public MinionBase _minion;
    public readonly float _spawnTimeBetweenMinions = 5f;//thời gian đợi giữa 2 lần spawn
    public int minionThreshold;//Số minion tối đa
    public abstract Vector3 FindSurroundPoints(Vector3 target);
    
    protected (Component_Health, GameUnit, Component_Move_Enemy) GetDefenseData(Collider target)
    {
        return (ComponentCache.GetHealthComponent(target),
            ComponentCache.GetGameUnit(target),
            ComponentCache.GetEnemyMoveComponent(target));
    }
}
