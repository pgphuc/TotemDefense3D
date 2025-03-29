using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Move_Enemy_Melee : StateBase<EnemyMeleeBase>
{
    public State_Move_Enemy_Melee(EnemyMeleeBase unit, StateMachine<EnemyMeleeBase> stateMachine) : base(unit, stateMachine)
    {
        
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _unit._moveComponent.StartMoving();
    }

    public override void OnExit()
    {
        base.OnExit();
        _unit._moveComponent.StopMoving();
    }

    public override void OnFrameUpdate()
    {
        base.OnFrameUpdate();
        _unit._moveComponent.Moving();
        if (_unit._moveComponent._dualingTarget == null && _unit._moveComponent._agent.isStopped)
        {
            _unit._moveComponent.StartMoving();
        }
        else if (_unit._moveComponent.ReadyToAttackBase())
        {
            _unit._moveComponent.OccupiedAttackPoint();
            _unit.StateMachine.ChangeState(_unit.AttackState);
        }
        else if (_unit._moveComponent.ReadyToAttackMinion())
        {
            _unit.StateMachine.ChangeState(_unit.AttackState);
        }
    }

    public override void OnPhysicsUpdate()
    {
        base.OnPhysicsUpdate();
        if (_unit._moveComponent.IsTargetPointOccupied())
        {
            _unit._moveComponent.StartMoving();
        }
    }

    public override void AnimationTriggerEvent(GameUnit.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }
}
