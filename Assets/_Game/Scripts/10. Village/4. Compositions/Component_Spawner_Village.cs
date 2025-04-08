using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component_Spawner_Village : Component_Spawner_Base
{
    public Component_Spawner_Village(VillageBase village, int minionThreshold)
    {
        _village = village;
        this.minionThreshold = minionThreshold;
    }

    private VillageBase _village;
    
    //spawning variables
    
    public int minionCount;//Số minion hiện có
    public int reinforcementNeeded;//Số minion cần để reinforcement
    
    public int reinforcementSpawned;//Số minion reinforcement đã spawn
    public int defenseSpawned;//Số minion defend đã spawn
    
    // minion coroutine 
    private Coroutine _regenerateCoroutine;
    private Coroutine _defendCoroutine;
    private Coroutine _reinforcementCoroutine;
    
    public override void OnInit()
    {
        base.OnInit();
        minionCount = 0;
        reinforcementNeeded = 0;
        
        reinforcementSpawned = 0;
        defenseSpawned = 0;

        _regenerateCoroutine = null;
        _defendCoroutine = null;
        _reinforcementCoroutine = null;
    }
    
    #region Check minion Count

    public bool NeedReinforcing()
    {
        reinforcementNeeded = MapManager.Instance.NumberOfMinionRequired();
        return reinforcementNeeded > 0;
    }
    public bool VillageNeedMinion()
    {
        return minionCount < minionThreshold && _regenerateCoroutine == null;
    }

    #endregion
    
    
    
    #region spawn functions
    
    public void RegenerateMinion()
    {
        _regenerateCoroutine = CoroutineManager.StartRoutine(RegenerateCouroutine());
    }
    private IEnumerator RegenerateCouroutine()
    {
        while (minionCount < minionThreshold)
        {
            yield return new WaitForSeconds(_spawnTimeBetweenMinions);
            minionCount++;
        }
        _regenerateCoroutine = null;
    }
    
    private void SpawnMinion(Vector3 position, MovingType type)
    {
        _minion =  SimplePool.Spawn<MinionMeleeBase>(_village._minionMeleePrefab.poolType, position, Quaternion.identity);
        _minion.minionType = MinionType.Village;
        _minion.movingType = type;
        _minion._spawner = this;
        _minion.OnInit();
        _village.SubcribeAllEvents(_minion);
        switch (type)
        {
            case MovingType.Reinforcing:
                reinforcementSpawned++;
                break;
            case MovingType.Defending:
                defenseSpawned++;
                break;
        }
    }

    public void MinionRetreat()
    {
        if (_defendCoroutine != null)
            CoroutineManager.StopRoutine(_defendCoroutine);
        if (_reinforcementCoroutine != null)
            CoroutineManager.StopRoutine(_reinforcementCoroutine);
    }
    
    public void Reinforcement()
    {
        _reinforcementCoroutine = CoroutineManager.StartRoutine(ReinforcementCouroutine());
    }

    private IEnumerator ReinforcementCouroutine()
    {
        while (reinforcementSpawned < minionCount && reinforcementSpawned < reinforcementNeeded)
        {
            BarrackBase target = _village._checkComponent.FindNearestAvailableBarrack();
            if (!target)
                break;
            
            //set vị trí spawn gần mục tiêu nhất
            SpawnMinion(FindSurroundPoints(target.transform.position), MovingType.Reinforcing);
            
            //để barrack đăng ký sự kiện chết của minion sinh ra để reinforce
            target.SubcribeAllEvents(_minion);
            yield return new WaitForSeconds(_spawnTimeBetweenMinions);
        }
        _reinforcementCoroutine = null;
    }
    public void DefendingBase()
    {
        _defendCoroutine = CoroutineManager.StartRoutine(DefendCouroutine());
    }

    private IEnumerator DefendCouroutine()
    {
        while (defenseSpawned < minionCount)
        {
            Collider target = _village._checkComponent.FindNearestAvailableEnemy();
            if (!target)
                break;
            
            //set vị trí spawn gần enemy nhất
            SpawnMinion(FindSurroundPoints(target.transform.position), MovingType.Defending);
            
            //Đặt mục tiêu di chuyển tới vị trí của enemy
            ValueTuple<Component_Health, GameUnit, Component_Move_Enemy> enemyData = GetDefenseData(target);
            _minion._moveComponent.MoveToDefending(enemyData);
            
            yield return new WaitForSeconds(_spawnTimeBetweenMinions);
        }
        _defendCoroutine = null;
    }
    public override Vector3 FindSurroundPoints(Vector3 target)
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
    
    #endregion
    

   
}
