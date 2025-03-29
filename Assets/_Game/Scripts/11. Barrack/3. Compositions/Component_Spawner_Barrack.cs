using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component_Spawner_Barrack : Component_Spawner_Base
{
    public Component_Spawner_Barrack(BarrackBase barrack, int minionThreshold)
    {
        _barrack = barrack;
        this.minionThreshold = minionThreshold;
    }

    private BarrackBase _barrack;

    public int minionCount;//Số minion hiện có 
    public int defenseSpawned;//Số minion defend đã spawn
    
    // minion coroutine 
    public Coroutine _defendCoroutine;
    public override void OnInit()
    {
        base.OnInit();
        minionCount = 0;
        defenseSpawned = 0;

        _defendCoroutine = null;
    }
    
    #region Check minion Count

    

    public int NumberOfMinionsNeeded()
    {
        return minionThreshold - minionCount;
    }

    #endregion

    public void MinionRetreat()
    {
        if (_defendCoroutine == null)
            return;
        CoroutineManager.StopRoutine(_defendCoroutine);
    }
    
    public void DefendingBarrack()
    {
        _defendCoroutine = CoroutineManager.StartRoutine(DefendingCoroutine());
    }

    private IEnumerator DefendingCoroutine()
    {
        while (BarrackHasMinion())
        {
            Collider target = _barrack._checkComponent.FindNearestAvailableEnemy();
            if (!target)
                break;
            
            //set vị trí spawn gần enemy nhất
            SpawnMinion(FindSurroundPoints(target.transform.position));
            
            //Đặt mục tiêu di chuyển tới vị trí của enemy
            ValueTuple<Component_Health, GameUnit, Component_Move_Enemy> enemyData = GetDefenseData(target);
            _minion._moveComponent.MoveToDefending(enemyData);
            
            
            yield return new WaitForSeconds(_spawnTimeBetweenMinions);
        }

        _defendCoroutine = null;
    }

    public bool BarrackHasMinion()
    {
        return minionCount > 0 && minionCount > defenseSpawned;
    }
    private void SpawnMinion(Vector3 position)
    {
        _minion = SimplePool.Spawn<MinionMeleeBase>(_barrack._minionMeleePrefab.poolType, position, Quaternion.identity);
        _minion.minionType = MinionType.Barrack;
        _minion.movingType = MovingType.Defending;
        _minion._spawner = this;
        _minion.OnInit();
        //đăng ký sự kiện
        _barrack.SubcribeAllEvents(_minion);
        defenseSpawned++;
    }
    public override Vector3 FindSurroundPoints(Vector3 target)
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
