using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class Component_Move_Minion :ComponentBase, IComponentMove
{
    public Component_Move_Minion(MinionBase owner, NavMeshAgent agent)
    {
        _owner = owner;
        _agent = agent;
    }
    public override void OnInit()
    {
        if (_owner.movingType != MovingType.Defending)
            _dualingTarget = null;
        _agent.enabled = true;
    }
    public List<Collider> enemyInRange = new List<Collider>(); 
    public MinionBase _owner;
    public NavMeshAgent _agent;
    public Vector3 _target { get; set; }
    public Component_Health _dualingTarget { get; set; }
    public void Moving()
    {
        _agent.destination = _target;
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
    // ReSharper disable Unity.PerformanceAnalysis
    public Vector3 FindDestination()
    {
        Vector3 destination = Vector3.zero;
        if (enemyInRange.Count > 0)
        {
            destination = FindNearestEnemy();
        }
        else
        {
            switch (_owner.movingType)
            {
                case MovingType.Reinforcing:
                    destination = FindBarrackAvailable();
                    break;
                case MovingType.Defending:
                case MovingType.AfterMatch://Sau khi giết đc enemy
                    switch (_owner.minionType)
                    {
                        case MinionType.Barrack:
                            destination = BackToBarrack();
                            break;
                        case MinionType.Village:
                            destination = BackToVillage();
                            break;
                    }
                    break;
            }
        }
        return destination;
    }

    private Vector3 FindBarrackAvailable()
    {
        Vector3 movePosition = MapManager.Instance.village.transform.position;
        bool hasBarrackAvailable = MapManager.Instance.CheckBarrackAvailability();
        if (hasBarrackAvailable)
        {
            int territoryID = int.MaxValue;
            foreach (BarrackBase barrack in MapManager.Instance.BarrackNotFullList)
            {
                if (barrack._territory.territoryID >= territoryID)
                    continue;
                territoryID = barrack._territory.territoryID;
                movePosition = barrack.transform.position;
            }
        }
        return movePosition;
    }

    private Vector3 FindNearestEnemy()
    {
        Vector3 movePosition = new Vector3();
        float minDistance = float.MaxValue;
        foreach (Collider target in enemyInRange)
        {
            float distance = Vector3.Distance(_owner.transform.position, target.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                movePosition = target.transform.position;
            }
        }
        return movePosition;
    }

    private Vector3 BackToVillage()
    {
        return _owner.villageSpawner.spawnerComponent.FindSurroundPoints(_owner.transform.position);
    }

    private Vector3 BackToBarrack()
    {
        return _owner.barrackSpawner.spawnerComponent.FindSurroundPoints(_owner.transform.position);
    }
}


