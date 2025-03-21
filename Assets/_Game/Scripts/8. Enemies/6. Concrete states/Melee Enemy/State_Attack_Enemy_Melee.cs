using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Attack_Enemy_Melee : StateBase<EnemyMeleeBase>
{
    public State_Attack_Enemy_Melee(EnemyMeleeBase unit, StateMachine<EnemyMeleeBase> stateMachine) : base(unit, stateMachine)
    {
       
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _unit._attackComponent._lastAttackTime = Time.time;
        _unit.SetState(this);
    }

    public override void OnExit()
    {
        base.OnExit();
        _unit._attackComponent.StopAttacking();
    }

    public override void OnFrameUpdate()
    {
        base.OnFrameUpdate();
        if (_unit._attackComponent._target == null || !_unit._attackComponent._target._isActive)
        {
            //_unit._attackSightCheck.HandleExit();
            //_unit._attackMeleeCheck.HandleExit();
            _unit.StateMachine.ChangeState(_unit.MoveState); // Quay lại tuần tra nếu không có mục tiêu
        }
        else if (Time.time - _unit._attackComponent._lastAttackTime >= _unit._attackComponent._speed)
        {
            _unit._attackComponent._lastAttackTime = Time.time;
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
