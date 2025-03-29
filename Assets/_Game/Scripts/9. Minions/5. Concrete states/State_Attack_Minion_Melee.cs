using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Attack_Minion_Melee :StateBase<MinionMeleeBase>
{
    public State_Attack_Minion_Melee(MinionMeleeBase unit, StateMachine<MinionMeleeBase> stateMachine) : base(unit, stateMachine)
    {
        
    }
    public override void OnEnter()
    {
        base.OnEnter();
        _unit._attackComponent.StartAttack();
    }

    public override void OnExit()
    {
        base.OnExit();
        _unit._attackComponent.StopAttacking();
    }

    public override void OnFrameUpdate()
    {
        base.OnFrameUpdate();
        if (_unit._attackComponent._attackTarget == null)
        {
            _stateMachine.ChangeState(_unit.MoveState); // Quay lại tuần tra nếu không có mục tiêu
        }
        else if (_unit._attackComponent.FinishCooldown())
        {
            _unit._attackComponent.MeleeAttack();
        }
    }

    public override void OnPhysicsUpdate()
    {
        base.OnPhysicsUpdate();
        if (!_unit._checkComponent._isFindingUnblockedEnemy)
            return;
        if (_unit._checkComponent.HasAvailableTarget())
        {
            _stateMachine.ChangeState(_unit.MoveState);
        }
    }

    public override void AnimationTriggerEvent(GameUnit.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }
}
