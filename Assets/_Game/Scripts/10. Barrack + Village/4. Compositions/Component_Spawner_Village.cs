using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component_Spawner_Village : ComponentBase
{
    public Component_Spawner_Village(VillageBase village)
    {
        _village = village;
    }
    public VillageBase _village;
    public MinionMeleeBase _minion;
    public float _spawnTime;
    public override void OnInit()
    {
        base.OnInit();
        _spawnTime = 5f;
    }

    private void SpawnMinion(Vector3 position)
    {
        _minion = SimplePool.Spawn<MinionMeleeBase>(_village._minionMeleePrefab.poolType, position, Quaternion.identity);
    }
    
    public void Reinforcement()
    {
        SpawnMinion(_village.transform.position);
        _minion.minionType = MinionType.Village;
        _minion.movingType = MovingType.Reinforcing;
        _minion.villageSpawner = _village;
        _minion.OnInit();
    }
    public void DefendingBase(Collider target)
    {
        
        SpawnMinion(FindSurroundPoints(target.transform.position));
        _minion.minionType = MinionType.Village;
        _minion.movingType = MovingType.Defending;
        _minion.villageSpawner = _village;
        _minion.OnInit();
        _minion._moveComponent.enemyInRange.Add(target);
    }

    public Vector3 FindSurroundPoints(Vector3 target)
    {
        Vector3 spawnPoint = _village.transform.position;
        float minDistance = float.MaxValue;
        foreach (Vector3 point in MapManager.Instance.surroundBasePoints.Keys)
        {
            float distance = Vector3.Distance(point, target);
            if (distance < minDistance)
            {
                minDistance = distance;
                spawnPoint = point;
            }
        }
        return spawnPoint;
    }
}
