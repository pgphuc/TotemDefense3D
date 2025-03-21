using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Attack_Totem : StateBase<TotemBase>
{
    public State_Attack_Totem(TotemBase unit, StateMachine<TotemBase> stateMachine) : base(unit, stateMachine)
    {
        
    }

    public override void OnEnter()
    {
        base.OnEnter();
        //Xóa toàn bộ enemy đã chết khỏi list
        _unit._attackComponent._targetList.RemoveAll(collider => !collider.gameObject.activeSelf);
        //TODO: start charging or calculate bullet path
    }

    public override void OnExit()
    {
        base.OnExit();
        _unit._attackComponent.StopAttacking();
    }

    public override void OnFrameUpdate()
    {
        base.OnFrameUpdate();
        _unit._attackComponent.Attack();
        _unit.stateMachine.ChangeState(_unit._cooldownState);
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
