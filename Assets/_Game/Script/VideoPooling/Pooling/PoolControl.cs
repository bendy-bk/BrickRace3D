using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolControl : MonoBehaviour
{
    [SerializeField] PoolAmount[] poolAmounts;
    // Start is called before the first frame update


    void Awake()
    {
        //GameUnit[] gameUnits = Resources.LoadAll<GameUnit>("Pool/");

        //// load tu resources
        //for (int i = 0; i < gameUnits.Length; i++)
        //{
        //    SimplePool.PreLoad(gameUnits[i], 0, new GameObject(gameUnits[i].name).transform);
        //}

        //Load tu list
        for (int i = 0; i < poolAmounts.Length; i++)
        {
            SimplePool.PreLoad(poolAmounts[i].prefab, poolAmounts[i].amount, poolAmounts[i].parrent);
        }

    }
}

[System.Serializable]
public class PoolAmount
{
    public GameUnit prefab;
    public Transform parrent;
    public int amount;
}

public enum PoolType
{
    Bullet_1 = 0,
    Bullet_2 = 1,
    Bullet_3 = 2,
}
