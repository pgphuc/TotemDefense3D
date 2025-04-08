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

    private EnemyBase _owner;
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
        _owner._animator.SetBool("isWalking", false);
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
    }

    public void StartMoving()
    {
        _owner._animator.SetBool("isWalking", true);
        _agent.isStopped = false;
        _owner._healthComponent._isBlocked = false;
        SetMoveTarget(FindDestination());
        
        _owner.InvokeExitAttack();
    }
    public void SetMoveTarget(Vector3 target)
    {
        _target = target;
    }

    public Vector3 FindDestination()
    {
        Vector3 bestPoint = MapManager.Instance.village.transform.position;
        switch (_owner)
        {
            case EnemyMeleeBase:
            case EnemyGiantBase:
                bestPoint = FindDestination_Melee();
                break;
            case EnemyBombBase:
                bestPoint = FindDestination_Bomb();
                break;
            case EnemyFlyBase:
                break;
        }
        return bestPoint;
    }
    
    private Vector3 FindDestination_Melee()
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

    private Vector3 FindDestination_Bomb()
    {
        Vector3 bestPoint = MapManager.Instance.village.transform.position;
        if (MapManager.Instance.BarrackList.Count > 0)
        {
            float minDistance = float.MaxValue;
            BarrackBase nearestBarrrack = null;
            foreach (BarrackBase barrack in MapManager.Instance.BarrackList)
            {
                float distance = Vector3.Distance(_owner.transform.position, barrack.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestBarrrack = barrack;
                }
            }
            _dualingTarget = nearestBarrrack.components.Find(_target => _target is Component_Health)
                as Component_Health;
            foreach (Vector3 point in nearestBarrrack.surroundBarrackPoints)
            {
                float distance = Vector3.Distance(_owner.transform.position, point);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    bestPoint = point;
                }
            }
        }
        else
        {
            GameUnit village = ComponentCache.GetGameUnit(MapManager.Instance.villageCollider);
            _dualingTarget = village.components.Find(_target => _target is Component_Health)
                as Component_Health;
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

    public bool ReadyToMeleeAttackBase()
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
        _owner.SubcribeAllEvents(GetTargetComponent.Item2);
        _owner.InvokeEnterAttack();
        
    }
    public bool ReadyToMeleeAttackMinion()
    {
        if (_dualingTarget?._owner is MinionBase)
        {
            return _agent.isStopped
                   && Vector3.Distance(_owner.transform.position, _dualingTarget._transform.position) <= _meleeRadius;
        }
        return false;
    }
}
