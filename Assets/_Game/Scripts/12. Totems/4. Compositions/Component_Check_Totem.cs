using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component_Check_Totem : ComponentBase
{
    public Component_Check_Totem(TotemBase owner)
    {
        _owner = owner;
    }
    private TotemBase _owner;
    private List<Collider> enemyInRange = new List<Collider>();
    private Transform _transform;
    public override void OnInit()
    {
        base.OnInit();
        _transform = MapManager.Instance.village.transform;
        enemyInRange.Clear();
    }
    public void HandleEnemyEnter(Collider enemyCollider)
    {
        if (!enemyInRange.Contains(enemyCollider))
        {
            enemyInRange.Add(enemyCollider);
        }
    }
    public void HandleEnemyExit(Collider enemyCollider)
    {
        if (enemyInRange.Contains(enemyCollider))
        {
            enemyInRange.Remove(enemyCollider);
        }
        if (_owner._attackComponent.EnemyKilledByTotem(enemyCollider))
        {
            _owner._attackComponent._attackTarget = null;
        }
    }

    public bool HasEnemyInRange()
    {
        return enemyInRange.Count > 0;
    }

    public Component_Health FindNearestEnemy()
    {
        Collider target = null;
        float minDistance = float.MaxValue;
        foreach (Collider enemy in enemyInRange)
        {
            float distance = Vector3.Distance(_transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                target = enemy;
            }
        }
        return ComponentCache.GetHealthComponent(target);
    }
}
