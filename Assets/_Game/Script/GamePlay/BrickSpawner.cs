using UnityEngine;

public class BrickSpawner : MonoBehaviour
{
    [Header("Pool settings")]
    public int prefabIndex = 0; // Index của prefab trong ObjectPoolManager

    [Header("Grid settings")]
    public int rows = 5;
    public int cols = 6;
    public float spacing = 1.5f;
    public Vector3 startPoint = new Vector3(-5f, 0f, -5f);

    [Header("Ground detection")]
    public float groundCheckHeight = 5f;
    public float groundOffset = 0.1f;

    [Header("Ngẫu nhiên spawn")]
    [Range(0f, 1f)]
    public float spawnChance = 1f; // 1 = 100% spawn, 0.5 = 50% spawn...

    private void Start()
    {
        SpawnGridBricks();
    }

    void SpawnGridBricks()
    {
        
    }
}
