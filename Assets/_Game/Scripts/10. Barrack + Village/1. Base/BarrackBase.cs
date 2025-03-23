using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BarrackBase : GameUnit
{
    #region GameUnit inheritance implementation

    public override void ComponentConstructor()
    {
        base.ComponentConstructor();
        spawnerComponent = new Component_Spawner_Barrack(this);
    }

    public override void OnInit()
    {
        base.OnInit();
        minionCapacity = 10;
        enemyCheck._owner = this;
        
        //defense
        defenseSpawned = 0;
        enemyInRange.Clear();

        FindSurroundBarrackPoints();
    }

    public override void InitAllComponents()
    {
        base.InitAllComponents();
    }
    #endregion

    [SerializeField] public Check_Enemy_Barrack enemyCheck;
    
    public GameUnit _minionMeleePrefab;
    private MinionMeleeBase _minion;
    
    [HideInInspector] public Territory _territory;
    [HideInInspector] public Component_Spawner_Barrack spawnerComponent;
    public HashSet<Vector3> surroundBarrackPoints = new HashSet<Vector3>();
    
    private int minionCapacity;
    
    public int _minionCapacity
    {
        get => minionCapacity;
        set
        {
            minionCapacity = Mathf.Clamp(value, 0, minionCapacity);
            switch (_territory.state)
            {
                case TerritoryState.BarrackBuilt when minionCapacity == 0:
                    MapManager.Instance.BarrackNotFullList.Remove(this);
                    _territory.ChangeState(TerritoryState.BarrackFull);
                    break;
                case TerritoryState.BarrackFull when minionCapacity > 0:
                    MapManager.Instance.BarrackNotFullList.Add(this);
                    _territory.ChangeState(TerritoryState.BarrackBuilt);
                    break;
            }
        }
    }

    //defense variables
    public List<Component_Health> enemyInRange = new List<Component_Health>();
    public int defenseCount;
    public int defenseSpawned;

    private void Update()
    {
        if (minionCapacity >= 10)
            return;
        defenseCount = 10 - minionCapacity;
        if (defenseCount >= enemyInRange.Count && enemyInRange.Count > defenseSpawned)
        {
            defenseSpawned++;
            spawnerComponent.DefendingBarrack(enemyInRange[^1]);
        }
    }

    
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
    
    
    
    
    
    
}
