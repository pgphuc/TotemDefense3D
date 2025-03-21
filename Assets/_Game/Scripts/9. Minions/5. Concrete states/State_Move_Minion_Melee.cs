using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Move_Minion_Melee : StateBase<MinionMeleeBase>
{
    public State_Move_Minion_Melee(MinionMeleeBase unit, StateMachine<MinionMeleeBase> stateMachine) : base(unit, stateMachine)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _unit._moveComponent.StartMoving();
        _unit.SetState(this);
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
        if (_unit._moveComponent._dualingTarget == null || !_unit._moveComponent._dualingTarget._isActive)
        {
            _unit._attackSightCheck.HandleExit();
            _unit._attackMeleeCheck.HandleExit();
        }
        if (_unit._attackMeleeCheck.IsOwnerInCheck)
        {
            if (_unit.movingType == MovingType.AfterMatch)
            {
                _unit.movingType = MovingType.Defending;
            }
            _unit.StateMachine.ChangeState(_unit.AttackState);
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
