using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TotemBase : GameUnit
{
    [SerializeField] private GameObject _shootArm;
    
    #region GameUnit implementation
    public override void Awake()
    {
        base.Awake();
    }

    public override void StateMachineConstructor()
    {
        base.StateMachineConstructor();
        stateMachine = new StateMachine<TotemBase>();
        _idleState = new State_Idle_Totem(this, stateMachine);
        _attackState = new State_Attack_Totem(this, stateMachine);
        _cooldownState = new State_Cooldown_Totem(this, stateMachine);
    }

    public override void ComponentConstructor()
    {
        base.ComponentConstructor();
        _healthComponent = new Component_Health(this, transform, 100f);
        _attackComponent = new Component_Attack_Totem(this, 8f, 3f, _shootArm);
    }

    public override void OnInit()
    {
        base.OnInit();
        _attackCheck._owner = this;
        stateMachine.Initialize(_idleState);
    }

    public override void InitAllComponents()
    {
        base.InitAllComponents();
        _healthComponent.OnInit();
        _attackComponent.OnInit();
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
    }
    #endregion

    #region Composite implementation

    public Component_Health _healthComponent;
    public Component_Attack_Totem _attackComponent;

    #endregion
    
    #region Statemachine implementation

    public StateMachine<TotemBase> stateMachine;
    public State_Idle_Totem _idleState;
    public State_Attack_Totem _attackState;
    public State_Cooldown_Totem _cooldownState;
    
    #endregion
    
    #region TriggerCheck implementation

    public Check_AttackRange_Totem _attackCheck;

    #endregion

    public virtual void Update()
    {
        stateMachine.currentState.OnFrameUpdate();
    }
    public virtual void FixedUpdate()
    {
        stateMachine.currentState.OnPhysicsUpdate();
    }

}
