using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Cooldown_Totem : StateBase<TotemBase>
{
    public State_Cooldown_Totem(TotemBase unit, StateMachine<TotemBase> stateMachine) : base(unit, stateMachine)
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
    }

    public override void OnFrameUpdate()
    {
        base.OnFrameUpdate();
        if (Time.time - _unit._attackComponent._lastAttackTime >= _unit._attackComponent._speed)
        {
            //TODO: spawn đạn mới
            _unit.stateMachine.ChangeState(_unit._idleState);
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
