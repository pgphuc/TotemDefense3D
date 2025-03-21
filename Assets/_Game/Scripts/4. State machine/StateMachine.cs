using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T> where T: GameUnit
{
    public StateBase<T> currentState;

    public void Initialize(StateBase<T> beginningState)//Call when spawning enemy
    {
        currentState = beginningState;
        currentState?.OnEnter();
    }

    public void ChangeState(StateBase<T> newState)
    {
        currentState?.OnExit();
        currentState = newState;
        currentState?.OnEnter();
    }
}
