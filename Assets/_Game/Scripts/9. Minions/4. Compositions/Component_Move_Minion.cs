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
    public Component_Move_Minion(MinionBase owner, NavMeshAgent agent, float attackRadius)
    {
        _owner = owner;
        _agent = agent;
        _meleeRadius = attackRadius;
    }
    public override void OnInit()
    {
        if (_owner.movingType != MovingType.Defending)
            _dualingTarget = null;
        _agent.enabled = true;
    }
    private MinionBase _owner;
    public NavMeshAgent _agent;
    public Vector3 _target { get; set; }
    public Component_Health _dualingTarget { get; set; }
    public float _meleeRadius { get; set; }
    public bool _isMovingToEnemy;
    public void Moving()
    {
        _agent.destination = _target;
    }

    public void StopMoving()
    {
        _agent.isStopped = true;
        _isMovingToEnemy = false;
        _agent.velocity = Vector3.zero;
    }

    public void StartMoving()
    {
        _agent.isStopped = false;
        _isMovingToEnemy = false;
        _owner._healthComponent._isBlocked = false;
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
        if (_owner._checkComponent._isFindingUnblockedEnemy || _owner._checkComponent.HasAvailableTarget())
        {
            _isMovingToEnemy = true;
            ValueTuple<Component_Health, GameUnit, Component_Move_Enemy> targetTuple = _owner._checkComponent.FindNearstTarget();
            BlockEnemy(targetTuple.Item3);
            SubcribeDeathEvent(targetTuple.Item2);
            _dualingTarget = targetTuple.Item1;
            destination = targetTuple.Item1._transform.position;
        }
        else
        {
            switch (_owner.movingType)
            {
                case MovingType.Reinforcing:
                    destination = FindBarrackAvailable();
                    break;
                case MovingType.Defending:
                    if (_dualingTarget != null)
                    {
                        destination = _dualingTarget._transform.position;
                    }
                    else 
                    {
                        goto case MovingType.AfterMatch;
                    }
                    break;
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
        if (destination == Vector3.zero)
        {
            Debug.LogError("No destination found");
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


    private Vector3 BackToVillage()
    {
        return _owner.villageSpawner.spawnerComponent.FindSurroundPoints(_owner.transform.position);
    }

    private Vector3 BackToBarrack()
    {
        return _owner.barrackSpawner.spawnerComponent.FindSurroundPoints(_owner.transform.position);
    }

    public void MoveToDefending((Component_Health, GameUnit, Component_Move_Enemy) GetDefenseData)
    {
        _isMovingToEnemy = true;
        SetDefenseTarget(GetDefenseData.Item1);
        SubcribeDeathEvent(GetDefenseData.Item2);
        BlockEnemy(GetDefenseData.Item3);
        
    }
    public void MoveToEnemy()
    {
        _isMovingToEnemy = true;
        ValueTuple<Component_Health, GameUnit, Component_Move_Enemy> targetTuple = _owner._checkComponent.FindNearstTarget();
        SetDefenseTarget(targetTuple.Item1);
        SubcribeDeathEvent(targetTuple.Item2);
        BlockEnemy(targetTuple.Item3);
    }

    private void SetDefenseTarget(Component_Health target)
    {
        _dualingTarget = target;
        SetMoveTarget(target._transform.position);
    }

    private void SubcribeDeathEvent(GameUnit sub)
    {
        _owner.SubDeathEvent(sub);
    }

    private void BlockEnemy(Component_Move_Enemy enemy)
    {
        ValueTuple<Component_Health, GameUnit> minionTargetTuple = GetTargetComponent();
        enemy.StopByMinion(minionTargetTuple);
    }

    private (Component_Health, GameUnit) GetTargetComponent()
    {
        return (_owner._healthComponent, _owner);
    }
    public bool ReadyToAttack()
    {
        return _dualingTarget != null
                && Vector3.Distance(_owner.transform.position, _dualingTarget._transform.position) <= _meleeRadius;
    }

    public void CheckMovingType()
    {
        if (_owner.movingType == MovingType.Defending)
        {
            _owner.movingType = MovingType.AfterMatch;
            StartMoving();
        }
        if (_agent.remainingDistance > _meleeRadius)
            return;
        switch (_owner.movingType)
        {
            case MovingType.AfterMatch:
                _owner.movingType = _owner.minionType == MinionType.Village ?
                    MovingType.ReachedVillage : MovingType.ReachedBarrack;
                break;
            case MovingType.Reinforcing:
                _owner.movingType = MovingType.ReachedBarrack;
                break;
        }
        _owner.OnDespawn();
    }
}


