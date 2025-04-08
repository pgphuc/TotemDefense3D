using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyBombBase : EnemyBase
{
    #region checklog variables
    //để sau
    #endregion
    
    
    [SerializeField] private Rigidbody rb;
    
    #region state machine references
    //State machine
    [HideInInspector] public StateMachine<EnemyBombBase> StateMachine;
    [HideInInspector] public State_Move_Enemy_Bomb MoveState;
    
    #endregion
    
    #region unity loop functions
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

    protected override void StateMachineConstructor()
    {
        StateMachine = new StateMachine<EnemyBombBase>();
        MoveState = new State_Move_Enemy_Bomb(this, StateMachine);
    }

    protected override void ComponentConstructor()
    {
        //health
        _healthComponent = new Component_Health(this, transform,3f);
        components.Add(_healthComponent);
        //attack
        _attackComponent = new Component_Attack_Enemy(this, 1f, 1f);
        components.Add(_attackComponent);
        //move
        _moveComponent = new Component_Move_Enemy(this, GetComponent<NavMeshAgent>(), 5f);
        components.Add(_moveComponent);
    }
    public override void OnInit()
    {
        base.OnInit();
        //State khởi đầu
        StateMachine.Initialize(MoveState);
        goldAmount = 1;
    }

    protected override void InitAllComponents()
    {
        _healthComponent.OnInit();
        _attackComponent.OnInit();
        _moveComponent.OnInit();
    }

    #endregion
    
    #region Animation Triggers implementation

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Barrack") && !other.CompareTag("Village"))
            return;
        _attackComponent._attackTarget = _moveComponent._dualingTarget;
        _attackComponent.BombAttack();
    }


    public void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        //TODO: fill in once stateMachine is created
        StateMachine.currentState.AnimationTriggerEvent(triggerType);
    }
    #endregion
}
