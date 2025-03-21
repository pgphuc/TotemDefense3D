using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component_Spawner_Barrack : ComponentBase
{
    public Component_Spawner_Barrack(BarrackBase barrack)
    {
        _barrack = barrack;
    }
    public BarrackBase _barrack;
    public MinionMeleeBase _minion;
    public override void OnInit()
    {
        base.OnInit();
    }
    private void SpawnMinion(Vector3 position)
    {
        _minion = SimplePool.Spawn<MinionMeleeBase>(_barrack._minionMeleePrefab.poolType, position, Quaternion.identity);
    }
    public void DefendingBarrack(Component_Health target)
    {
        SpawnMinion(FindSurroundPoints(target._transform.position));
        _minion.minionType = MinionType.Barrack;
        _minion.movingType = MovingType.Defending;
        _minion.barrackSpawner = _barrack;
        _minion.OnInit();
        _minion._moveComponent._dualingTarget = target;
    }
    public Vector3 FindSurroundPoints(Vector3 target)
    {
        Vector3 spawnPoint = _barrack.transform.position;
        float minDistance = float.MaxValue;
        foreach (Vector3 point in _barrack.surroundBarrackPoints)
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
