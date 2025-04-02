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
        //TODO: Change anim/sound/...
        //TODO: start charging or calculate bullet path
        _unit._attackComponent.StartAttack();
    }

    public override void OnExit()
    {
        base.OnExit();
        //TODO: Change anim/sound/...
        _unit._attackComponent.StopAttacking();
    }

    public override void OnFrameUpdate()
    {
        base.OnFrameUpdate();
        if (_unit._attackComponent._attackTarget == null || _unit._attackComponent._attackTarget._isActive == false)
        {
            _stateMachine.ChangeState(_unit._idleState);
        }
        else if (_unit._attackComponent.FinishCoolDown())
        {
            _unit._attackComponent.Attack(); 
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
