using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Idle_Barrack : StateBase<BarrackBase>
{
    public State_Idle_Barrack(BarrackBase unit, StateMachine<BarrackBase> stateMachine) : base(unit, stateMachine)
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
        if (_unit._checkComponent.NeedDefending())
        {
            _stateMachine.ChangeState(_unit._defendingState);
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
