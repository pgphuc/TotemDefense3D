using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRangedBase : EnemyBase
{
    #region Bullet references
    public GameUnit _bulletPrefab;
    #endregion
    
    #region stateMachine references
    //State machine
    [HideInInspector] public StateMachine<EnemyRangedBase> StateMachine;
    [HideInInspector] public State_Move_Enemy_Ranged MoveState;
    [HideInInspector] public State_RangedAttack_Enemy_Ranged RangedAttackState;
    [HideInInspector] public State_MeleeAttack_Enemy_Ranged MeleeAttackState;
    
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
    
    public override void OnInit()
    {
        base.OnInit();
        //State khởi đầu
        StateMachine.Initialize(MoveState);
    }

    protected override void StateMachineConstructor()
    {
        StateMachine = new StateMachine<EnemyRangedBase>();
        MoveState = new State_Move_Enemy_Ranged(this, StateMachine);
        RangedAttackState = new State_RangedAttack_Enemy_Ranged(this, StateMachine);
        MeleeAttackState = new State_MeleeAttack_Enemy_Ranged(this, StateMachine);
    }

    protected override void ComponentConstructor()
    {
        //health
        _healthComponent = new Component_Health(this, transform, 10f);
        components.Add(_healthComponent);
        //attack
        _attackComponent = new Component_Attack_Enemy(this, 1f, 1.5f);
        components.Add(_attackComponent);
        //move
        _moveComponent = new Component_Move_Enemy(this, GetComponent<NavMeshAgent>(), 1f);
        components.Add(_moveComponent);
        _checkComponent = new Component_Check_EnemyRanged(this);
        components.Add(_checkComponent);
    }

    protected override void InitAllComponents()
    {
        _healthComponent.OnInit();
        _attackComponent.OnInit();
        _moveComponent.OnInit();
        _checkComponent.OnInit();
    }

    #endregion

    #region Event implementations

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("MeleeMinion") && !other.CompareTag("Totem"))
            return;
        _checkComponent.HandleEnter(other);
        SubcribeAllEvents(ComponentCache.GetGameUnit(other));
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("MeleeMinion") && !other.CompareTag("Totem"))
            return;
        _checkComponent.HandleExit(other);
        UnSubcribeAllEvents(ComponentCache.GetGameUnit(other));
    }
    protected override void OnTargetDeath(GameUnit unit)
    {
        base.OnTargetDeath(unit);
        if (unit is not MinionBase)
            return;
        Collider target = ComponentCache.GetCollider(unit);
        _checkComponent.HandleExit(target);
        if (_attackComponent.EnemyKilledByOwner(target))
        {
            _attackComponent._attackTarget = null;
        }
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
