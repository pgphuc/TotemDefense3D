using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component_Check_EnemyRanged : ComponentBase
{
    public Component_Check_EnemyRanged(EnemyBase owner)
    {
        _owner = owner;
    }
    private EnemyBase _owner;
    private List<Collider> minionInRange = new List<Collider>();
    public override void OnInit()
    {
        base.OnInit();
        minionInRange.Clear();
    }
    public void HandleEnter(Collider minion)
    {
        if (!minionInRange.Contains(minion))
        {
            minionInRange.Add(minion);
        }
    }
    public void HandleExit(Collider minion)
    {
        if (minionInRange.Contains(minion))
        {
            minionInRange.Remove(minion);
        }
        if (_owner._attackComponent.EnemyKilledByOwner(minion))
        {
            _owner._attackComponent._attackTarget = null;
        }
    }

    public bool HasMinionInRange()
    {
        return minionInRange.Count > 0;
    }

    public Component_Health FindNearestEnemy()
    {
        Collider target = null;
        float minDistance = float.MaxValue;
        foreach (Collider minion in minionInRange)
        {
            float distance = Vector3.Distance(_owner.transform.position, minion.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                target = minion;
            }
        }
        return ComponentCache.GetHealthComponent(target);
    }
}
