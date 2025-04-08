using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Move_Enemy_Ranged : StateBase<EnemyRangedBase>
{
    public State_Move_Enemy_Ranged(EnemyRangedBase unit, StateMachine<EnemyRangedBase> stateMachine) : base(unit, stateMachine)
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
        if (_unit._checkComponent.HasTargetInRange())
        {
            _unit.StateMachine.ChangeState(_unit.RangedAttackState);
        }
        else if (_unit._moveComponent.ReadyToMeleeAttackMinion())
        {
            _unit.StateMachine.ChangeState(_unit.MeleeAttackState);
        }
    }

    public override void OnPhysicsUpdate()
    {
        base.OnPhysicsUpdate();
    }

    public override void AnimationTriggerEvent(GameUnit.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }
}
