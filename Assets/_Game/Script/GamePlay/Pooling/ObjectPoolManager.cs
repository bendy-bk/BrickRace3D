using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    [SerializeField] private List<PoolItem> poolItems;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        InitPools();
    }

    void InitPools()
    {
        foreach (var item in poolItems)
        {
            for (int i = 0; i < item.poolSize; i++)
            {
                GameObject obj = Instantiate(item.prefab);
                obj.SetActive(false);
                item.poolQueue.Enqueue(obj);
            }
        }
    }

    // Lấy object theo index prefab (0 = prefab đầu tiên, 1 = thứ hai,...)
    public GameObject GetFromPool(int prefabIndex)
    {
        PoolItem item = poolItems[prefabIndex];

        if (item.poolQueue.Count > 0)
        {
            GameObject obj = item.poolQueue.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        // Hết thì tạo thêm nếu cần
        GameObject newObj = Instantiate(item.prefab);
        return newObj;
    }

    public void ReturnToPool(GameObject obj, int prefabIndex)
    {
        obj.SetActive(false);
        poolItems[prefabIndex].poolQueue.Enqueue(obj);
    }
}
