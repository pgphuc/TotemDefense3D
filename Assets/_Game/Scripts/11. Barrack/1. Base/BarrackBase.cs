using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class BarrackBase : GameUnit
{
    #region checklog variables
    //để sau
    #endregion
    
    #region prefabs
    public GameUnit _minionMeleePrefab;
    private MinionBase _minion;
    #endregion
    
    #region others
    public HashSet<Vector3> surroundBarrackPoints = new HashSet<Vector3>();
    private void FindSurroundBarrackPoints()
    {
        float distance = 1f;
        Vector3 grid = new Vector3(10f , 2f, 10f);
        List<Vector3> points = new List<Vector3>();
        for (float x = transform.position.x - grid.x / 2 - distance; x < transform.position.x + grid.x / 2 + distance; x += distance)
        {
            points.Add(new Vector3(x, transform.position.y, transform.position.z - grid.z/2 - distance));//cạnh dưới
            points.Add(new Vector3(x, transform.position.y, transform.position.z + grid.z/2 + distance));//cạnh trên
        }
        for (float z = transform.position.z - grid.z / 2 - distance; z < transform.position.z + grid.z / 2 + distance; z += distance)
        {
            points.Add(new Vector3(transform.position.x - grid.x/2 - distance, transform.position.y, z));//cạnh trái
            points.Add(new Vector3(transform.position.x + grid.x/2 + distance, transform.position.y, z));//cạnh phải
        }
        foreach (Vector3 point in points)
        {
            if (NavMesh.SamplePosition(point, out NavMeshHit hit, Mathf.Infinity, NavMesh.AllAreas))
            {
                surroundBarrackPoints.Add(hit.position);
            }
        }
    }
    #endregion
    
    #region component references
    [HideInInspector] public Component_Health _healthComponent;
    [HideInInspector] public Component_Spawner_Barrack _spawnerComponent;
    [HideInInspector] public Component_Check_Defender _checkComponent;
    #endregion
    
    #region State machine references
    [HideInInspector] public StateMachine<BarrackBase> _stateMachine;
    [HideInInspector] public State_Defending_Barrack _defendingState; 
    [HideInInspector] public State_Idle_Barrack _idleState;
    #endregion
    
    #region unity loop
    private void Update()
    {
        _stateMachine.currentState.OnFrameUpdate();
    }
    private void FixedUpdate()
    {
        _stateMachine.currentState.OnPhysicsUpdate();
    }
    
    #endregion
    
    #region GameUnit inheritance implementation

    protected override void StateMachineConstructor()
    {
        base.StateMachineConstructor();
        _stateMachine = new StateMachine<BarrackBase>();
        _idleState = new State_Idle_Barrack(this, _stateMachine);
        _defendingState = new State_Defending_Barrack(this, _stateMachine);
    }

    protected override void ComponentConstructor()
    {
        base.ComponentConstructor();
        _spawnerComponent = new Component_Spawner_Barrack(this, 10);
        components.Add(_spawnerComponent);
        _checkComponent = new Component_Check_Defender(transform, _spawnerComponent);
        components.Add(_checkComponent);
        _healthComponent = new Component_Health(this, transform, 1f);
    }

    public override void OnInit()
    {
        base.OnInit();
        FindSurroundBarrackPoints();
        _stateMachine.Initialize(_idleState);
    }

    protected override void InitAllComponents()
    {
        base.InitAllComponents();
        _spawnerComponent.OnInit();
        _checkComponent.OnInit();
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
            case EnemyBase:
                _checkComponent.HandleEnemyExit(ComponentCache.GetCollider(unit));
                break;
            case MinionBase minion:
                if (minion.minionType == MinionType.Village)
                {
                    _spawnerComponent.minionCount++;
                }
                else
                {
                    _spawnerComponent.defenseSpawned--;
                    if (minion.movingType == MovingType.Defending)
                        _spawnerComponent.minionCount--;
                }
                break;
        }
    }
    #endregion
}
