using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component_Check_Minion : ComponentBase
{
    public Component_Check_Minion(GameUnit owner, Transform transform, float checkRadius)
    {
        _owner = owner;
        _transform = transform;
        _radius = checkRadius;
    }
    private GameUnit _owner;
    private Transform _transform;
    private float _radius;
    public bool _isFindingUnblockedEnemy;
    
    private readonly Collider[] _checkResults = new Collider[20];
    public List<Collider> _targetsInRange = new List<Collider>();
    
    public override void OnInit()
    {
        base.OnInit();
        _isFindingUnblockedEnemy = false;
    }
    
    public bool HasAvailableTarget()
    {
        GetTargetsInRange();
        return _targetsInRange.Count > 0;
    }
    
    private void GetTargetsInRange()
    {
         int count = Physics.OverlapSphereNonAlloc(_transform.position, _radius, _checkResults, LayerMask.GetMask("Enemy"));
         _targetsInRange.Clear();
         for (int i = 0; i < count; i++)
         {
             if (_isFindingUnblockedEnemy)
             {
                 if (ComponentCache.GetHealthComponent(_checkResults[i])._isBlocked)
                     continue;
             }
             _targetsInRange.Add(_checkResults[i]);
         }
    }
    public (Component_Health, GameUnit, Component_Move_Enemy) FindNearstTarget()
    {
         Collider targetCollider = null;
         float minDistance = float.MaxValue;
         foreach (Collider target in _targetsInRange)
         {
             float distance = Vector3.Distance(_transform.position, target.transform.position);
             if (distance >= minDistance)
                 continue;
             minDistance = distance;
             targetCollider = target;
         }
         return (ComponentCache.GetHealthComponent(targetCollider),
             ComponentCache.GetGameUnit(targetCollider),
             ComponentCache.GetEnemyMoveComponent(targetCollider));
    }
}
