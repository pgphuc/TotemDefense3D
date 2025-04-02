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
        // _unit._moveComponent.Moving();
        // // if (!_unit._attackComponent._target || !_unit._attackComponent._target._isActive)
        // // {
        // //     _unit._attackRangedCheck.HandleExit();
        // //     _unit._attackMeleeCheck.HandleExit();
        // // }
        // if (_unit._attackMeleeCheck.IsOwnerInCheck)
        // {
        //     _unit.StateMachine.ChangeState(_unit.MeleeAttackState);//Đánh cận chiến
        // }
        // else if (_unit._attackRangedCheck.IsOwnerInCheck)
        // {
        //     _unit.StateMachine.ChangeState(_unit.RangedAttackState);//Đánh xa
        // }
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
