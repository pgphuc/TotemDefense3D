using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Idle_Village : StateBase<VillageBase>
{
    public State_Idle_Village(VillageBase unit, StateMachine<VillageBase> stateMachine) : base(unit, stateMachine)
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
        if (_unit._spawnerComponent.minionCount <=0)
            return;
        if (_unit._checkComponent.NeedDefending())
        {
            _stateMachine.ChangeState(_unit._defendingState);
        }
        else if (_unit._spawnerComponent.NeedReinforcing())
        {
            _stateMachine.ChangeState(_unit._reinforcingState);
        }
        
    }

    public override void OnPhysicsUpdate()
    {
        base.OnPhysicsUpdate();
        if (_unit._spawnerComponent.VillageNeedMinion())
        {
            _unit._spawnerComponent.RegenerateMinion();
        }
    }

    public override void AnimationTriggerEvent(GameUnit.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }
}
