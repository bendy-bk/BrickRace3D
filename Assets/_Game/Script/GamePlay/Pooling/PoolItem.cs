using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolItem
{
    public GameObject prefab;
    public int poolSize = 10;

    [HideInInspector] public Queue<GameObject> poolQueue = new Queue<GameObject>();
}
