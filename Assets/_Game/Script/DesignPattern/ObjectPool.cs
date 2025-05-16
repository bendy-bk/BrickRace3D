using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private T prefab;
    private Transform parent;
    private Queue<T> pool = new Queue<T>();

    public ObjectPool(T prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;
        for (int i = 0; i < initialSize; i++)
            pool.Enqueue(CreateNew());
    }

    private T CreateNew()
    {
        var go = GameObject.Instantiate(prefab.gameObject, parent);
        go.SetActive(false);
        return go.GetComponent<T>();
    }

    public T Get()
    {
        if (pool.Count == 0) pool.Enqueue(CreateNew());
        var obj = pool.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Release(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}
