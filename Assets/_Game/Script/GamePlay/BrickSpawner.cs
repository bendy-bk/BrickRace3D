using UnityEngine;

public class BrickSpawner : MonoBehaviour
{
    public string brickTag = "BrickPlayer";

    public int rows = 5;                  // Số hàng
    public int cols = 6;                  // Số cột
    public float spacing = 1.5f;          // Khoảng cách giữa các viên brick
    public Vector3 startPoint = new Vector3(-5f, 0f, -5f); // Gốc của lưới brick (điểm bắt đầu)

    public float groundCheckHeight = 5f;
    public float groundOffset = 0.1f;



    void Start()
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

                // Raycast xuống đất để đảm bảo vị trí nằm trên Ground
                Vector3 rayOrigin = spawnPos + Vector3.up * groundCheckHeight;
                if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, groundCheckHeight + 1f))
                {
                    if (hit.collider.CompareTag("Ground"))
                    {
                        Vector3 finalPos = hit.point + Vector3.up * groundOffset;
                        Debug.Log(finalPos);
                        Debug.Log(brickTag);
                        Debug.Log(Quaternion.identity);

                        PoolManager.Instance.SpawnFromPool(brickTag, finalPos, Quaternion.identity);
                    }
                }
            }
        }
    }
}
