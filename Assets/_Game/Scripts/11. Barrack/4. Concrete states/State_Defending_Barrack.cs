using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Defending_Barrack : StateBase<BarrackBase>
{
    public State_Defending_Barrack(BarrackBase unit, StateMachine<BarrackBase> stateMachine) : base(unit, stateMachine)
    {
        
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _unit._spawnerComponent.DefendingBarrack();
        //TODO: Change anim/sound/...
    }

    public override void OnExit()
    {
        base.OnExit();
        _unit._spawnerComponent.MinionRetreat();
        //TODO: Change anim/sound/...
    }

    public override void OnFrameUpdate()
    {
        base.OnFrameUpdate();
        if (!_unit._checkComponent.NeedDefending())
        {
            _stateMachine.ChangeState(_unit._idleState);
        }
    }

    public override void OnPhysicsUpdate()
    {
        base.OnPhysicsUpdate();
        if (_unit._spawnerComponent._defendCoroutine != null)
            return;
        if (_unit._spawnerComponent.BarrackHasMinion())
        {
            _unit._spawnerComponent.DefendingBarrack();
        }
    }

    public override void AnimationTriggerEvent(GameUnit.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }
}
