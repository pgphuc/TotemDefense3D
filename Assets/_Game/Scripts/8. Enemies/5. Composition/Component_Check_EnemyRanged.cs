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
    private List<Collider> targetInRange = new List<Collider>();
    public override void OnInit()
    {
        base.OnInit();
        targetInRange.Clear();
    }
    public void HandleEnter(Collider minion)
    {
        if (!targetInRange.Contains(minion))
        {
            targetInRange.Add(minion);
        }
    }
    public void HandleExit(Collider minion)
    {
        if (targetInRange.Contains(minion))
        {
            targetInRange.Remove(minion);
        }
        if (_owner._attackComponent.EnemyKilledByOwner(minion))
        {
            _owner._attackComponent._attackTarget = null;
        }
    }

    public bool HasTargetInRange()
    {
        return targetInRange.Count > 0;
    }

    public bool HasMinionInRange()
    {
        for (int i = 0; i < targetInRange.Count; i++)
        {
            var target = ComponentCache.GetMinion(targetInRange[i]);
            if (target)
            {
                return true;
            }
        }
        return false;
    }

    public Component_Health FindNearestEnemy()
    {
        Collider target = null;
        float minDistance = float.MaxValue;
        foreach (Collider minion in targetInRange)
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
