using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Move_Enemy_Bomb : StateBase<EnemyBombBase>
{
    public State_Move_Enemy_Bomb(EnemyBombBase unit, StateMachine<EnemyBombBase> stateMachine) : base(unit, stateMachine)
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
        _unit._moveComponent.Moving();
        if (_unit._moveComponent._dualingTarget == null || _unit._moveComponent._dualingTarget._isActive == false)
        {
            _unit._moveComponent.StartMoving();
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
