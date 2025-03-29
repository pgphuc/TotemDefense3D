using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Defending_Village : StateBase<VillageBase>
{
    public State_Defending_Village(VillageBase unit, StateMachine<VillageBase> stateMachine) : base(unit, stateMachine)
    {
        
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _unit._spawnerComponent.DefendingBase();
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
        if (!_unit._spawnerComponent.NeedDefending())
        {
            _stateMachine.ChangeState(_unit._idleState);
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
