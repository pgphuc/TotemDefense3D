using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TotemBase : GameUnit
{
    #region checklog variables
    //để sau
    #endregion
    
    #region Component references

    public Component_Health _healthComponent;
    public Component_Attack_Totem _attackComponent;
    public Component_Check_Totem _checkComponent;

    #endregion
    
    #region Statemachine references

    public StateMachine<TotemBase> stateMachine;
    public State_Idle_Totem _idleState;
    public State_Attack_Totem _attackState;
    
    #endregion
    
    #region Bullet references
    public GameUnit _bulletPrefab;
    public Transform _bulletSpawnPoint;
    
    #endregion
    

    #region unity loop
    public virtual void Update()
    {
        stateMachine.currentState.OnFrameUpdate();
    }
    public virtual void FixedUpdate()
    {
        stateMachine.currentState.OnPhysicsUpdate();
    }
    #endregion
    #region GameUnit implementation

    protected override void StateMachineConstructor()
    {
        base.StateMachineConstructor();
        stateMachine = new StateMachine<TotemBase>();
        _idleState = new State_Idle_Totem(this, stateMachine);
        _attackState = new State_Attack_Totem(this, stateMachine);
    }

    protected override void ComponentConstructor()
    {
        base.ComponentConstructor();
        _healthComponent = new Component_Health(this, transform, 100f);
        components.Add(_healthComponent);
        _checkComponent = new Component_Check_Totem(this);
        components.Add(_checkComponent);

    }

    public override void OnInit()
    {
        base.OnInit();
        stateMachine.Initialize(_idleState);
    }

    protected override void InitAllComponents()
    {
        base.InitAllComponents();
        _healthComponent.OnInit();
        _attackComponent.OnInit();
        _checkComponent.OnInit();
    }
    #endregion

    #region event implementation
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("MeleeEnemy") && !other.CompareTag("RangedEnemy"))
            return;
        _checkComponent.HandleEnemyEnter(other);
        SubcribeAllEvents(ComponentCache.GetGameUnit(other));
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("MeleeEnemy") && !other.CompareTag("RangedEnemy"))
            return;
        _checkComponent.HandleEnemyExit(other);
        UnSubcribeAllEvents(ComponentCache.GetGameUnit(other));
    }
    
    
    protected override void OnTargetDeath(GameUnit unit)
    {
        base.OnTargetDeath(unit);
        if (unit is not EnemyBase)
            return;
        Collider target = ComponentCache.GetCollider(unit);
        _checkComponent.HandleEnemyExit(target);
        if (_attackComponent.EnemyKilledByTotem(target))
        {
            _attackComponent._attackTarget = null;
        }
        
    }
    #endregion
    

}
