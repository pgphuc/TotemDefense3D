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
        _unit._attackComponent.StartMeleeAttack();
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
            _unit.StateMachine.ChangeState(_unit.MoveState); //Quay lại tuần tra nếu không có mục tiêu
        }
        else if (_unit._attackComponent.FinishCooldown())
        {
            _unit._attackComponent.MeleeAttack();
        }
        
    }

    public override void OnPhysicsUpdate()
    {
        base.OnPhysicsUpdate();
        if (_unit._attackComponent.IsAttackingStructure() && _unit._moveComponent.ReadyToMeleeAttackMinion())
        {
            _unit._attackComponent.StartMeleeAttack();
        }
    }

    public override void AnimationTriggerEvent(GameUnit.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }
}
