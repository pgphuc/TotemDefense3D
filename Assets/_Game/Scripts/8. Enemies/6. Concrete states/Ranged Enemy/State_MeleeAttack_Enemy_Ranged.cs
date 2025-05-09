using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_MeleeAttack_Enemy_Ranged : StateBase<EnemyRangedBase>
{
    public State_MeleeAttack_Enemy_Ranged(EnemyRangedBase unit, StateMachine<EnemyRangedBase> stateMachine) : base(unit, stateMachine)
    {
        
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _unit._attackComponent._lastAttackTime = Time.time;
    }

    public override void OnExit()
    {
        base.OnExit();
        _unit._attackComponent.StopAttacking();
    }

    public override void OnFrameUpdate()
    {
        base.OnFrameUpdate();
        if (_unit._attackComponent._attackTarget == null || _unit._attackComponent._attackTarget._isActive == false)
        {
            _unit.StateMachine.ChangeState(_unit.MoveState); // Quay lại tuần tra nếu không có mục tiêu
        }
        else if (_unit._attackComponent.FinishCooldown())
        {
            _unit._attackComponent.MeleeAttack();
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
