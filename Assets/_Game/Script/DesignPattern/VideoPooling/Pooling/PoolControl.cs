using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolControl : MonoBehaviour
{

    [SerializeField] PoolAmount[] poolAmounts;
    private List<Vector3> randomPositions = new List<Vector3>();

    public List<Vector3> RandomPositions { get => randomPositions; set => randomPositions = value; }

    // Start is called before the first frame update


    void Awake()
    {
        randomPositions = GenerateRandomPositions(10, - 15f, 15f, -17f, 17f);

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

        SpawnObjectsAtRandomPositions();

    }

    public List<Vector3> GenerateRandomPositions(int amount, float minX, float maxX, float minZ, float maxZ, float fixedY = 0.1f)
    {
       
        for (int i = 0; i < amount; i++)
        {
            float x = Random.Range(minX, maxX);
            float z = Random.Range(minZ, maxZ);
            Vector3 pos = new Vector3(x, fixedY, z);
            RandomPositions.Add(pos);
        }
      
        return RandomPositions;
    }

    public void SpawnObjectsAtRandomPositions()
    {
        if (randomPositions.Count == 0)
        {
            Debug.LogWarning("Random positions list is empty!");
            return;
        }

        int posIndex = 0;

        for (int i = 0; i < poolAmounts.Length; i++)
        {
            PoolAmount pool = poolAmounts[i];

            for (int j = 0; j < pool.amount; j++)
            {
                if (posIndex >= randomPositions.Count)
                {
                    Debug.LogWarning("Không còn vị trí random để gán đối tượng!");
                    break;
                }

                Vector3 spawnPos = randomPositions[posIndex];
                Quaternion spawnRot = Quaternion.identity;

                // Gọi hàm Spawn với kiểu T là GameUnit
                GameUnit obj = SimplePool.Spawn<GameUnit>(pool.prefab.poolType, spawnPos, spawnRot);

                if (obj != null)
                {
                    obj.transform.SetParent(pool.parrent, worldPositionStays: true);
                    posIndex++;
                }
                else
                {
                    Debug.LogWarning($"Spawn đối tượng từ pool {pool.prefab.poolType} thất bại!");
                }
            }
        }
    }


}



[System.Serializable]
internal class PoolAmount
{
    public GameUnit prefab;
    public Transform parrent;
    public int amount;
}

