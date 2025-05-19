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
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Vector3 spawnPos = startPoint + new Vector3(col * spacing, 0, row * spacing);

                // Raycast từ trên cao xuống mặt đất
                Vector3 rayOrigin = spawnPos + Vector3.up * groundCheckHeight;
                if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, groundCheckHeight + 1f))
                {
                    if (hit.collider.CompareTag("Ground"))
                    {
                        Vector3 finalPos = hit.point + Vector3.up * groundOffset;

                        if (Random.value <= spawnChance)
                        {
                            GameObject brick = ObjectPoolManager.Instance.GetFromPool(prefabIndex);
                            brick.transform.position = finalPos;
                            brick.transform.rotation = Quaternion.identity;
                            brick.SetActive(true);
                        }
                    }
                }
            }
        }
    }
}
