using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBase<T> : IState where T : GameUnit
{
    protected T _unit;
    protected StateMachine<T> _stateMachine;

    public StateBase(T unit, StateMachine<T> stateMachine)
    {
        _unit = unit;
        _stateMachine = stateMachine;
    }

    public virtual void OnEnter()//Gọi khi enter 1 state
    {
    
    }

    public virtual void OnExit()//Gọi khi kết thúc 1 state
    {
    
    }

    public virtual void OnFrameUpdate()//Gọi trong update để thực hiện state
    {
    
    }

    public virtual void OnPhysicsUpdate()//Gọi trong fixedUpdate để thực hiện tác động vật lý
    {
    
    }

    public virtual void AnimationTriggerEvent(GameUnit.AnimationTriggerType triggerType)//Được gọi khi chạy animation
    {
    
    }
}
