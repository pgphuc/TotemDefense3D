using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Move_Minion_Melee : StateBase<MinionMeleeBase>
{
    public State_Move_Minion_Melee(MinionMeleeBase unit, StateMachine<MinionMeleeBase> stateMachine) : base(unit, stateMachine)
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
        if (_unit._moveComponent._dualingTarget == null && _unit._moveComponent._isMovingToEnemy)
        {
            _unit._moveComponent.StartMoving();
        }
        if (_unit._moveComponent.ReadyToAttack())
        {
            _unit.StateMachine.ChangeState(_unit.AttackState);
        }
    }

    public override void OnPhysicsUpdate()
    {
        base.OnPhysicsUpdate();
        if (_unit._moveComponent._dualingTarget != null)
            return;
        if (_unit._checkComponent.HasAvailableTarget())
        {
            _unit._moveComponent.MoveToEnemy();
        }
        else
        {
            _unit._moveComponent.CheckMovingType();
        }
    }

    public override void AnimationTriggerEvent(GameUnit.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }
}
