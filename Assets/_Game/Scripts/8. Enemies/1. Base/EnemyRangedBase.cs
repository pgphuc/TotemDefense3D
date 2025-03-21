using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRangedBase : EnemyBase
{
    [SerializeField] private string CurrentState;

    public void SetState(StateBase<EnemyRangedBase> state)
    {
        CurrentState = state.ToString();
    }
    
    #region Serialized Fields
    //TriggerCheck
    public Check_AttackMelee_Enemy _attackMeleeCheck;
    public Check_AttackRanged_Enemy _attackRangedCheck;
    
    #endregion
    
    #region variables that need instantiate at Awake [HideInInspector]
    //State machine
    [HideInInspector] public StateMachine<EnemyRangedBase> StateMachine;
    [HideInInspector] public State_Move_Enemy_Ranged MoveState;
    [HideInInspector] public State_RangedAttack_Enemy_Ranged RangedAttackState;
    [HideInInspector] public State_MeleeAttack_Enemy_Ranged MeleeAttackState;
    
    #endregion
    
    #region Awake / Update / FixedUpdate
    public override void Awake()
    {
        base.Awake();
    }
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
    
    public override void OnInit()
    {
        base.OnInit();
        //TriggerCheck
        _attackMeleeCheck._attackComponent = _attackComponent;
        _attackRangedCheck._attackComponent = _attackComponent;
        //State khởi đầu
        StateMachine.Initialize(MoveState);
    }

    public override void StateMachineConstructor()
    {
        StateMachine = new StateMachine<EnemyRangedBase>();
        MoveState = new State_Move_Enemy_Ranged(this, StateMachine);
        RangedAttackState = new State_RangedAttack_Enemy_Ranged(this, StateMachine);
        MeleeAttackState = new State_MeleeAttack_Enemy_Ranged(this, StateMachine);
    }
    public override void ComponentConstructor()
    {
        //health
        _healthComponent = new Component_Health(this, transform, 110f);
        components.Add(_healthComponent);
        //attack
        _attackComponent = new Component_Attack_Enemy(this, 10f, 1.5f);
        components.Add(_attackComponent);
        //move
        _moveComponent = new Component_Move_Enemy(this, GetComponent<NavMeshAgent>());
        components.Add(_moveComponent);
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
