using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeBase : EnemyBase
{
    [SerializeField] private string CurrentState;
    [SerializeField] private string readytoattack;
    [SerializeField] private string dualingTarget;
    [SerializeField] private string distance;

    public void SetState(StateBase<EnemyMeleeBase> state)
    {
        CurrentState = state.ToString();
    }
    
    
    #region variables that need instantiate at Awake [HideInInspector]
    //State machine
    [HideInInspector] public StateMachine<EnemyMeleeBase> StateMachine;
    [HideInInspector] public State_Move_Enemy_Melee MoveState;
    [HideInInspector] public State_Attack_Enemy_Melee AttackState;
    
    #endregion
    
    #region Awake / Update / FixedUpdate
    public override void Awake()
    {
        base.Awake();
    }
    public virtual void Update()
    {
        StateMachine.currentState.OnFrameUpdate();
        readytoattack = _moveComponent.ReadyToAttackMinion().ToString();
        if (_moveComponent._dualingTarget == null)
            return;
        dualingTarget = _moveComponent._dualingTarget._transform.position.ToString();
        distance = Vector3.Distance(transform.position, _moveComponent._dualingTarget._transform.position).ToString();
    }

    public virtual void FixedUpdate()
    {
        StateMachine.currentState.OnPhysicsUpdate();
    }
    
    #endregion
    
    #region GameUnit override functions

    public override void StateMachineConstructor()
    {
        StateMachine = new StateMachine<EnemyMeleeBase>();
        MoveState = new State_Move_Enemy_Melee(this, StateMachine);
        AttackState = new State_Attack_Enemy_Melee(this, StateMachine);
    }
    public override void ComponentConstructor()
    {
        //health
        _healthComponent = new Component_Health(this, transform,15f);
        components.Add(_healthComponent);
        //attack
        _attackComponent = new Component_Attack_Enemy(this, 3f, 1.5f);
        components.Add(_attackComponent);
        //move
        _moveComponent = new Component_Move_Enemy(this, GetComponent<NavMeshAgent>(), 3f);
        components.Add(_moveComponent);
    }
    public override void OnInit()
    {
        base.OnInit();
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
