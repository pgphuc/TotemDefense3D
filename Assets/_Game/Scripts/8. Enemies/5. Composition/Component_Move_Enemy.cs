using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Component_Move_Enemy : ComponentBase, IComponentMove
{
    public Component_Move_Enemy(EnemyBase owner, NavMeshAgent agent)
    {
        _owner = owner;
        _agent = agent;
    }
    public override void OnInit()
    {
        _dualingTarget = null;
        _agent.enabled = true;
    }
    public List<Collider> _minionInRange = new List<Collider>();
    public EnemyBase _owner;
    public NavMeshAgent _agent;
    public Vector3 _target { get; set; }
    public Component_Health _dualingTarget { get; set; }
    public void Moving()
    {
        _agent.destination = _target;
        if (!MapManager.Instance.surroundBasePoints.TryGetValue(_target, out bool isOccupied))
        {
            return;
        }
        if (isOccupied)
        {
            SetMoveTarget(FindDestination());
        }
        if (_agent.remainingDistance < 3f && _agent.remainingDistance > 0)
        {
            MapManager.Instance.surroundBasePoints[_target] = true;
        }
    }

    public void StopMoving()
    {
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
    }

    public void StartMoving()
    {
        _agent.isStopped = false;
        SetMoveTarget(FindDestination());
    }
    public void SetMoveTarget(Vector3 target)
    {
        _target = target;
    }
    public Vector3 FindDestination()
    {
        // Vector3 destination = new Vector3();
        // destination = _minionInRange.Count > 0 ? FindNearestTarget() : FindSurroundBasePoint();
        // return destination;
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

    public Vector3 FindSurroundBasePoint()
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
    public Vector3 FindNearestTarget()
    {
        float minDistance = float.MaxValue;
        Vector3 nearestTarget = MapManager.Instance.village.transform.position;
        foreach (Collider target in _minionInRange)
        {
            float distance = Vector3.Distance(_owner.transform.position, target.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestTarget = target.transform.position;
            }
        }
        return nearestTarget;
    }
}
