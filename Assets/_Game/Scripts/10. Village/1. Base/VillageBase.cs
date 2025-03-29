using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class VillageBase : GameUnit
{
    #region checklog variables
    //để sau
    #endregion
    
    #region prefabs
    public GameUnit _minionMeleePrefab;
    
    #endregion
    
    #region Component references
    [HideInInspector] public Component_Health _healthComponent;
    [HideInInspector] public Component_Spawner_Village _spawnerComponent;
    [HideInInspector] public Component_Check_Defender _checkComponent;
    #endregion
    
    #region State machine references
    [HideInInspector] public StateMachine<VillageBase> _stateMachine;
    [HideInInspector] public State_Reinforcing_Village _reinforcingState;
    [HideInInspector] public State_Defending_Village _defendingState;
    [HideInInspector] public State_Idle_Village _idleState;
    #endregion
    
    #region unity loop
    private void Start()
    {
        OnInit();
    }
    public void Update()
    {
        _stateMachine.currentState.OnFrameUpdate();
    }

    public void FixedUpdate()
    {
        _stateMachine.currentState.OnPhysicsUpdate();
    }
    #endregion
    
    #region GameUnit Implementation
    protected override void StateMachineConstructor()
    {
        base.StateMachineConstructor();
        _stateMachine = new StateMachine<VillageBase>();
        _reinforcingState = new State_Reinforcing_Village(this, _stateMachine);
        _defendingState = new State_Defending_Village(this, _stateMachine);
        _idleState = new State_Idle_Village(this, _stateMachine);
    }

    protected override void ComponentConstructor()
    {
        base.ComponentConstructor();
        _spawnerComponent = new Component_Spawner_Village(this, 20);
        components.Add(_spawnerComponent);
        _healthComponent = new Component_Health(this, transform, 100f);
        components.Add(_healthComponent);
        _checkComponent = new Component_Check_Defender(transform, _spawnerComponent);
        components.Add(_checkComponent);
    }
    public override void OnInit()
    {
        base.OnInit();
        _stateMachine.Initialize(_idleState);
    }

    protected override void InitAllComponents()
    {
        base.InitAllComponents();
        _spawnerComponent.OnInit();
        _healthComponent.OnInit();
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

    public override void SubcribeAllEvents(GameUnit unit)
    {
        base.SubcribeAllEvents(unit);
        if (unit is not EnemyBase)
            return;
        unit.EnterAttack += _checkComponent.HandleEnemyEnterAttack;
        unit.ExitAttack += _checkComponent.HandleEnemyExitAttack;

    }

    protected override void UnSubcribeAllEvents(GameUnit unit)
    {
        base.UnSubcribeAllEvents(unit);
        if (unit is not EnemyBase)
            return;
        unit.EnterAttack -= _checkComponent.HandleEnemyEnterAttack;
        unit.ExitAttack -= _checkComponent.HandleEnemyExitAttack;
    }

    protected override void OnTargetDeath(GameUnit unit)
    {
        base.OnTargetDeath(unit);
        switch (unit)
        {
            case EnemyBase enemy:
                _checkComponent.HandleEnemyExit(ComponentCache.GetCollider(unit));
                if (MapManager.Instance.surroundBasePoints.TryGetValue(enemy._moveComponent._target, out bool isOccupied))
                {
                    if (isOccupied)
                    {
                        MapManager.Instance.surroundBasePoints[enemy._moveComponent._target] = false;
                    }
                }
                break;
            case MinionBase minion:
                switch (minion.movingType)
                {
                    case MovingType.ReachedVillage:
                        _spawnerComponent.defenseSpawned--;
                        break;
                    default:
                        _spawnerComponent.minionCount--;
                        if (minion.movingType == MovingType.Defending)
                        {
                            _spawnerComponent.defenseSpawned--;
                        }
                        else
                        {
                            _spawnerComponent.reinforcementSpawned--;
                        }
                        break;
                }
                break;
        }
    }
    
    #endregion
    
    
}
