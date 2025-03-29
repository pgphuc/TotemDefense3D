using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component_Check_Defender : ComponentBase
{
    public Component_Check_Defender(Transform transform, Component_Spawner_Base spawner)
    {
        _transform = transform;
        _spawner = spawner;
    }

    private Transform _transform;
    private Component_Spawner_Base _spawner;
    public Dictionary<Collider, bool> enemyDictionary = new Dictionary<Collider, bool>();
    private List<Collider> enemyList = new List<Collider>();

    public override void OnInit()
    {
        base.OnInit();
        enemyDictionary.Clear();
        enemyList.Clear();
    }
    public void HandleEnemyEnter(Collider enemyCollider)
    {
        if (!IsEnemyDictFull())
        {
            enemyDictionary.TryAdd(enemyCollider, false);
        }
        else if (!enemyList.Contains(enemyCollider))
        {
            enemyList.Add(enemyCollider);
        }
    }
    public void HandleEnemyExit(Collider enemyCollider)
    {
        if (enemyDictionary.ContainsKey(enemyCollider))
        {
            enemyDictionary.Remove(enemyCollider);
            EnsureEnemyDictIsFull();
        }
        else if (enemyList.Contains(enemyCollider))
        {
            enemyList.Remove(enemyCollider);
        }
    }
    public void HandleEnemyEnterAttack(GameUnit target)
    {
        if (enemyDictionary.ContainsKey(ComponentCache.GetCollider(target)))
        {
            enemyDictionary[ComponentCache.GetCollider(target)] = true;
        }
    }
    public void HandleEnemyExitAttack(GameUnit target)
    {
        if (enemyDictionary.ContainsKey(ComponentCache.GetCollider(target)))
        {
            enemyDictionary[ComponentCache.GetCollider(target)] = false;
        }
    }

    public Collider FindNearestAvailableEnemy()
    {
        float minDistance = float.MaxValue;
        Collider target = null;
        foreach (Collider key in enemyDictionary.Keys)
        {
            if (enemyDictionary[key])
                continue;
            float distance = Vector3.Distance(_transform.position, key.transform.position);
            if (distance <= minDistance)
            {
                minDistance = distance;
                target = key;
            }
        }
        return target;
    }
    public BarrackBase FindNearestAvailableBarrack()
    {
        float minDistance = float.MaxValue;
        BarrackBase target = null;
        foreach (BarrackBase barrack in MapManager.Instance.BarrackNotFullList)
        {
            float distance = Vector3.Distance(_transform.position, barrack.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                target = barrack;
            }
        }
        return target;
    }
    public bool NeedDefending()//Làm sau khi đến check của barrack
    {
        return enemyDictionary.Count > 0;
    }
    private bool IsEnemyDictFull()
    {
        return enemyDictionary.Count >= _spawner.minionThreshold;
    }
    private void EnsureEnemyDictIsFull()
    {
        while (!IsEnemyDictFull())
        {
            if (enemyList.Count == 0)
                return;
            enemyDictionary.TryAdd(enemyList[0], false);
            enemyList.RemoveAt(0);
        }
    }
    

    
}
