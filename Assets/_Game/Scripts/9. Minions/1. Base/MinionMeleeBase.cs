using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionMeleeBase : MinionBase
{
    #region checklog variables
    //để sau
    [SerializeField] private string currentStateName;
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
        currentStateName = StateMachine.currentState.ToString();

    }

    public virtual void FixedUpdate()
    {
        StateMachine.currentState.OnPhysicsUpdate();
    }
    
    #endregion
    
    #region GameUnit override functions

    protected override void StateMachineConstructor()
    {
        StateMachine = new StateMachine<MinionMeleeBase>();
        MoveState = new State_Move_Minion_Melee(this, StateMachine);
        AttackState = new State_Attack_Minion_Melee(this, StateMachine);
    }

    protected override void ComponentConstructor()
    {
        //health
        _healthComponent = new Component_Health(this,transform, 7f);
        components.Add(_healthComponent);
        //attack
        _attackComponent = new Component_Attack_Minion(this, 1f, 1f);
        components.Add(_attackComponent);
        //move
        _moveComponent = new Component_Move_Minion(this, GetComponent<NavMeshAgent>(), 3f, 6.5f);
        components.Add(_moveComponent);   
        //check
        _checkComponent = new Component_Check_Minion(transform, 5f);
        components.Add(_checkComponent);
    }
    public override void OnInit()
    {
        base.OnInit();
        //State khởi đầu
        StateMachine.Initialize(MoveState);
    }

    protected override void InitAllComponents()
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
