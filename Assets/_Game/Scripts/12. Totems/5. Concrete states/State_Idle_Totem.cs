using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class State_Idle_Totem : StateBase<TotemBase>
{
    public State_Idle_Totem(TotemBase unit, StateMachine<TotemBase> stateMachine) : base(unit, stateMachine)
    {
        
    }

    public override void OnEnter()
    {
        base.OnEnter();
        //TODO: Change anim/sound/...
    }

    public override void OnExit()
    {
        base.OnExit();
        //TODO: Change anim/sound/...
    }

    public override void OnFrameUpdate()
    {
        base.OnFrameUpdate();
        if (_unit._checkComponent.HasEnemyInRange())
        {
            _unit.stateMachine.ChangeState(_unit._attackState);
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
