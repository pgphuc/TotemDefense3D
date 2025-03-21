using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionMeleeBase : MinionBase
{
    #region checklog state machine
    [SerializeField] private string CurrentState;

    public void SetState(StateBase<MinionMeleeBase> state)
    {
        CurrentState = state.ToString();
    }
    #endregion
    
    #region Serialized Fields
    //TriggerCheck
    public Check_AttackMelee_Minion _attackMeleeCheck;
    public Check_AttackSight_Minion _attackSightCheck;
    
    #endregion
    
    #region variables that need instantiate at Awake [HideInInspector]
    //State machine
    
    [HideInInspector] public StateMachine<MinionMeleeBase> StateMachine;
    [HideInInspector] public State_Move_Minion_Melee MoveState;
    [HideInInspector] public State_Attack_Minion_Melee AttackState;
    
    #endregion
    
    #region Awake / Update / FixedUpdate
    public virtual void Update()
    {
        StateMachine.currentState.OnFrameUpdate();
    }

    public virtual void FixedUpdate()
    {
        StateMachine.currentState.OnPhysicsUpdate();
    }
    
    #endregion
    
    #region GameUnit override functions

    public override void StateMachineConstructor()
    {
        StateMachine = new StateMachine<MinionMeleeBase>();
        MoveState = new State_Move_Minion_Melee(this, StateMachine);
        AttackState = new State_Attack_Minion_Melee(this, StateMachine);
    }

    public override void ComponentConstructor()
    {
        //health
        _healthComponent = new Component_Health(this,transform, 12f);
        components.Add(_healthComponent);
        //attack
        _attackComponent = new Component_Attack_Minion(this, 3f, 1.5f);
        components.Add(_attackComponent);
        //move
        _moveComponent = new Component_Move_Minion(this, GetComponent<NavMeshAgent>());
        components.Add(_moveComponent);   
    }
    public override void OnInit()
    {
        base.OnInit();
        //move
        _moveComponent._agent.enabled = true;
        //TriggerCheck
        _attackMeleeCheck._attackComponent = _attackComponent;
        _attackSightCheck._moveComponent = _moveComponent;
        //State khởi đầu
        StateMachine.Initialize(MoveState);
    }

    public override void InitAllComponents()
    {
        _healthComponent.OnInit();
        _attackComponent.OnInit();
        _moveComponent.OnInit();
    }
    #endregion
    
    #region Animation Triggers implementation
    public void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        //TODO: fill in once stateMachine is created
        StateMachine.currentState.AnimationTriggerEvent(triggerType);
    }
    #endregion
    
}
