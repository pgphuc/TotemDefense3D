using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class SimplePool //1 class tồn tại duy nhất trong project
{
    //Thư viện lưu các pool
    private static Dictionary<PoolType, Pool> poolInstance = new Dictionary<PoolType, Pool>();

    //Phương thức để tạo pool với prefab và số lượng được truyền vào
    public static void PreLoad(GameUnit prefab, int amount, Transform parent)
    {
        if (prefab == null)
        {
            Debug.LogError($"POOL TYPE {prefab.poolType} IS NOT PRELOAD !!! ");
            return;
        }

        if (!poolInstance.ContainsKey(prefab.poolType) || poolInstance[prefab.poolType] == null)//Nếu pool chưa có hoặc bị null
        {
            Pool pool = new Pool();
            poolInstance[prefab.poolType] = pool;
            pool.PreLoad(prefab, amount, parent);
        }
    }

    
    //Hàm để spawn theo từng pool
    public static T Spawn<T>(PoolType poolType, Vector3 position, Quaternion rotation) where T : GameUnit
    {
        if (!poolInstance.ContainsKey(poolType) || poolInstance[poolType] == null)
        {
            Debug.LogError($"POOL TYPE {poolType} IS NOT SPAWN !!! ");
            return null;
        }
        return poolInstance[poolType].Spawn(position, rotation) as T;

    }

    
    //Hm để despawn
    public static void Despawn(GameUnit unit)
    {
        if (!poolInstance.ContainsKey(unit.poolType))
        {
            Debug.LogError($"POOL TYPE {unit.poolType} IS NOT DESPAWN !!! ");
        }
        poolInstance[unit.poolType].Despawn(unit);
    }
    
    //Trả phần từ về inactive của 1 pool
    public static void Collect(PoolType poolType)
    {
        if (!poolInstance.ContainsKey(poolType))
        {
            Debug.LogError($"POOL TYPE {poolType} IS NOT COLLECT !!! ");
        }
        poolInstance[poolType].Collect();
    }

    //Trả phần tử về inactive của TẤT CẢ pool
    public static void CollectAll()
    {
        foreach (var pool in poolInstance.Values)
        {
            pool.Collect();
        }
    }
    
    public static void Release(PoolType poolType)
    {
        if (!poolInstance.ContainsKey(poolType))
        {
            Debug.LogError($"POOL TYPE {poolType} IS NOT RELEASE !!! ");
        }
        poolInstance[poolType].Release();
    }

    public static void ReleaseAll()
    {
        foreach (var pool in poolInstance.Values)
        {
            pool.Release();
        }
    }
    
}
public class Pool
{
    private Transform parent;
    private GameUnit prefab;
    Queue<GameUnit> inactives = new Queue<GameUnit>();
    List<GameUnit> actives = new List<GameUnit>();

    //Khởi tạo pool mới
    public void PreLoad(GameUnit prefab, int amount, Transform parent)
    {
        this.parent = parent;
        this.prefab = prefab;
        for (int i = 0; i < amount; i++)
        {
            Despawn(GameObject.Instantiate(prefab, parent, true));
        }
    }
    
    //Lấy phần tử từ trong pool
    public GameUnit Spawn(Vector3 position, Quaternion rotation)
    {
        GameUnit unit;
        if (inactives.Count <= 0)
        {
            Despawn(GameObject.Instantiate(prefab, parent, true));
        }
        unit = inactives.Dequeue();
        actives.Add(unit);
        unit.transform.SetPositionAndRotation(position, rotation);
        unit.gameObject.SetActive(true);
        return unit;
    }

    //Trả phần tử về inactive
    public void Despawn(GameUnit unit)
    {
        if (unit != null && unit.gameObject.activeSelf)
        {
            if (actives.Count > 0)
            {
                actives.Remove(unit);
            }
            inactives.Enqueue(unit);
            unit.gameObject.SetActive(false);
        }
    }

    // lấy toàn bộ từ active về inactive
    public void Collect()
    {
        while (actives.Count > 0)
        {
            Despawn(actives[0]);
        }
    }

    //destroy toàn bộ inactive
    public void Release()
    {
        Collect();
        while (inactives.Count > 0)
        {
            GameObject.Destroy(inactives.Dequeue().gameObject);
        }
        inactives.Clear();
    }
}