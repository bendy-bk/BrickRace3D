using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public static class SimplePool
{
    private static Dictionary<PoolType, Pool> poolInstance = new Dictionary<PoolType, Pool>();

    // Khoi tao pool
    public static void PreLoad(GameUnit prefab, int amount, Transform parrent)
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab null");
            return;
        }

        if (!poolInstance.ContainsKey(prefab.poolType) || poolInstance[prefab.poolType] == null)
        {
            Pool p = new Pool();
            p.Preload(prefab, amount, parrent);
            poolInstance[prefab.poolType] = p;
        }
    }


    // Lay phan tu
    public static T Spawn<T>(PoolType poolType, Vector3 pos, Quaternion rot) where T : GameUnit
    {
        if (!poolInstance.ContainsKey(poolType))
        {
            Debug.LogError(poolType + "Is NOT Preload !!!");
            return null;

        }

        return poolInstance[poolType].Spawn(pos, rot) as T;

    }

    // tra phan tu vao
    public static void Despawn(GameUnit u)
    {
        if (!poolInstance.ContainsKey(u.poolType))
        {
            Debug.LogError(u.poolType + "Is NOT Preload !!!");

        }

        poolInstance[u.poolType].DesSpawn(u);


    }

    // thu thap 1 phan tu
    public static void Collect(PoolType poolType)
    {
        if (!poolInstance.ContainsKey(poolType))
        {
            Debug.LogError(poolType + "Is NOT Preload !!!");

        }

        poolInstance[poolType].Collect();
    }

    // Thu thap all
    public static void CollectAll()
    {
        foreach (var item in poolInstance.Values)
        {
            item.Collect();
        }

    }

    //Destroy 1, 
    public static void Release(PoolType poolType)
    {
        if (!poolInstance.ContainsKey(poolType))
        {
            Debug.LogError(poolType + "Is NOT Preload !!!");

        }

        poolInstance[poolType].Release();
    }
    //destroy all
    public static void ReleaseAll()
    {
        foreach (var item in poolInstance.Values)
        {
            item.Release();
        }
    }

}


public class Pool
{
    Transform parent;
    GameUnit prefab;
    // Queue chua cac unit dang o trong pool
    Queue<GameUnit> inactives = new Queue<GameUnit>();

    // List chua cac unit dang dc su dung
    List<GameUnit> actives = new List<GameUnit>();

    //Khoi tao pool
    public void Preload(GameUnit prefab, int amount, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < amount; i++)
        {
            DesSpawn(GameObject.Instantiate(prefab, parent));
        }
    }

    //get element from pool
    public GameUnit Spawn(Vector3 pos, Quaternion rot)
    {
        GameUnit unit;

        if (inactives.Count <= 0)
        {
            unit = GameObject.Instantiate(prefab, parent);
        }
        else
        {
            unit = inactives.Dequeue();
        }
        unit.TF.SetPositionAndRotation(pos, rot);
        actives.Add(unit);
        unit.gameObject.SetActive(true);

        return unit;
    }

    // return element to pool
    public void DesSpawn(GameUnit g)
    {
        if (g != null && g.gameObject.activeSelf)
        {
            actives.Remove(g);
            inactives.Enqueue(g);
            g.gameObject.SetActive(false);
        }
    }

    // Thu thập các phần tử đang dùng về pool
    public void Collect()
    {
        while (actives.Count > 0)
        {
            DesSpawn(actives[0]);
        }
    }

    // Destroy all element
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
