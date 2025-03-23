using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Component_Move_Enemy : ComponentBase, IComponentMove
{
    public Component_Move_Enemy(EnemyBase owner, NavMeshAgent agent, float attackRadius)
    {
        _owner = owner;
        _agent = agent;
        _meleeRadius = attackRadius;
    }
    public override void OnInit()
    {
        _dualingTarget = null;
        _agent.enabled = true;
    }
    public EnemyBase _owner;
    public NavMeshAgent _agent;
    public Vector3 _target { get; set; }
    public Component_Health _dualingTarget { get; set; }
    public float _meleeRadius { get; set; }
    public void Moving()
    {
        _agent.destination = _target;
    }

    public void StopMoving()
    {
        if (!_owner.isActiveAndEnabled)
            return;
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
    }

    public void StartMoving()
    {
        _agent.isStopped = false;
        _owner._healthComponent._isBlocked = false;
        SetMoveTarget(FindDestination());
    }
    public void SetMoveTarget(Vector3 target)
    {
        _target = target;
    }
    public Vector3 FindDestination()
    {
        Vector3 bestPoint = MapManager.Instance.village.transform.position;
        bool hasPointAvailable = MapManager.Instance.surroundBasePoints.ContainsValue(false);
        if (hasPointAvailable)
        {
            float minDistance = float.MaxValue;
            foreach (KeyValuePair<Vector3, bool> surroundPoint in MapManager.Instance.surroundBasePoints)
            {
                if (surroundPoint.Value)
                    continue;
                float distanceBetweenPoints = Vector3.Distance(_owner.transform.position, surroundPoint.Key);
                if (distanceBetweenPoints < minDistance)
                {                                                   
                    minDistance = distanceBetweenPoints;
                    bestPoint = surroundPoint.Key;
                }
            }
        }
        return bestPoint;
    }

    public bool IsTargetPointOccupied()
    {
        return MapManager.Instance.surroundBasePoints.TryGetValue(_target, out bool isOccupied) && isOccupied;
    }

    public bool ReadyToAttackBase()
    {
        if (!MapManager.Instance.surroundBasePoints.ContainsKey(_target))
            return false;
        return Vector3.Distance(_owner.transform.position , _target) <= _meleeRadius;
    }
    
    public void OccupiedAttackPoint()
    {
        _dualingTarget = ComponentCache.GetHealthComponent(MapManager.Instance.villageCollider);
        MapManager.Instance.surroundBasePoints[_target] = true;
    }
    

    public void StopByMinion((Component_Health, GameUnit) GetTargetComponent)
    {
        StopMoving();
        _dualingTarget = GetTargetComponent.Item1;
        SetMoveTarget(GetTargetComponent.Item1._transform.position);
        _owner.SubDeathEvent(GetTargetComponent.Item2);
        
    }
    public bool ReadyToAttackMinion()
    {
        if (_dualingTarget._owner is MinionBase)
        {
            return _dualingTarget != null
                   && _agent.isStopped
                   && Vector3.Distance(_owner.transform.position, _dualingTarget._transform.position) <= _meleeRadius;
        }
        return false;
    }
}
