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
    public Component_Move_Minion(MinionBase owner, NavMeshAgent agent, float attackRadius, float reinforceRadius)
    {
        _owner = owner;
        _agent = agent;
        _meleeRadius = attackRadius;
        _reinforceRadius = reinforceRadius;
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
    public float _reinforceRadius;
    public bool _isMovingToEnemy;
    public void Moving()
    {
        _agent.destination = _target;
    }

    public void StopMoving()
    {
        _owner._animator.SetBool("isWalking", false);
        _agent.isStopped = true;
        _isMovingToEnemy = false;
        _agent.velocity = Vector3.zero;
    }

    public void StartMoving()
    {
        _owner._animator.SetBool("isWalking", true);
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
        if ((_owner._checkComponent._isFindingUnblockedEnemy && _owner._checkComponent.HasAvailableTarget())||
            _owner._checkComponent.HasAvailableTarget())
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
                case MovingType.Defending when _dualingTarget != null:
                    destination = _dualingTarget._transform.position;
                    break;
                default:
                    destination = _owner.minionType == MinionType.Barrack ? BackToBarrack() : BackToVillage();
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
        if (!hasBarrackAvailable)
            return movePosition;
        float minDistance = float.MaxValue;
        foreach (BarrackBase barrack in MapManager.Instance.BarrackList)
        {
            float distance = Vector3.Distance(_owner.transform.position, barrack.transform.position);
            if (distance >= minDistance)
                continue;
            minDistance = distance;
            movePosition = barrack.transform.position;
        }
        return movePosition;
    }


    private Vector3 BackToVillage()
    {
        return _owner._spawner.FindSurroundPoints(_owner.transform.position);
    }

    private Vector3 BackToBarrack()
    {
        return _owner._spawner.FindSurroundPoints(_owner.transform.position);
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
        _owner.SubcribeAllEvents(sub);
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
        if (Vector3.Distance(_owner.transform.position, _target) > _reinforceRadius)
            return;
        switch (_owner.movingType)
        {
            case MovingType.Defending:
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


