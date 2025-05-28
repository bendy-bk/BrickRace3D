
using System.Buffers.Text;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Mesh Renderer")]
    [SerializeField] private Renderer mrenderer;
    [SerializeField] private ColorDataSO colorDataSO;
    [SerializeField] private ColorType colorType;

    [Header("List Brick Behind player")]
    [SerializeField] private Transform brickListRoot;
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private float brickHeight = 0.2f;

    private List<GameObject> bricks = new();

    public ColorType ColorType { get => colorType; set => colorType = value; }
    public ColorDataSO ColorDataSO { get => colorDataSO; set => colorDataSO = value; }

    public void OnInit()
    {

    }

    public void OnDespawn(GameObject g)
    {
        Destroy(g);
    }

    public void ChangeColor(ColorType colorType)
    {
        this.ColorType = colorType;
        mrenderer.material = ColorDataSO.GetMaterial(colorType);
    }

    public void AddBrick(BrickSpawn brick)
    {


        int index = bricks.Count;
        float yOffset = index * brickHeight;

        Debug.Log($"Brick: {brick.BrickColorType}");
        Debug.Log($"Player: {ColorType}");


        if (brick.BrickColorType == ColorType)
        {
            GameObject newBrick = Instantiate(brickPrefab, brickListRoot);
            newBrick.transform.localPosition = new Vector3(0, yOffset, 0);

            var col = newBrick.GetComponent<Collider>();
            if (col != null) col.enabled = false;

            bricks.Add(newBrick);

        }
        else
        {
            Debug.Log("Diff ColorType");
        }

        brick.gameObject.SetActive(false);

    }

    public void RemoveBrick()
    {
     
        if (GetStackCount() == 0) return;

        GameObject lastBrick = bricks[bricks.Count - 1];
        bricks.RemoveAt(bricks.Count - 1);
        
        Destroy(lastBrick);
        
        
    }

    public void ClearStack()
    {
        foreach (Transform child in brickListRoot)
        {
            Destroy(child.gameObject);
        }
        bricks.Clear();
    }

    public int GetStackCount() => bricks.Count;

    public void RaycastDown45Degrees(Vector3 rayOrigin)
    {
        // Hướng chiếu xuống dưới theo góc -45 độ (forward + down)
        Vector3 rayDirection = - transform.forward.normalized;

        // Raycast
        RaycastHit hit;
        bool isHit = Physics.Raycast(rayOrigin + new Vector3(0f, 0.3f, 0f), rayDirection, out hit, 2f);
        Debug.Log(hit.collider);
        if (isHit)
        {
            Debug.DrawLine(rayOrigin + new Vector3(0f, 0.3f, 0f), rayDirection, Color.red);
            Debug.Log($"Raycast hit: {hit.collider.name}");

            // Nếu muốn lấy script từ object bị hit
            var brick = hit.collider.GetComponent<BrickStair>();
            if (brick != null)
            {
                Debug.Log($"Brick Color: {brick.BrickColorType}");
            }
        }
        else
        {
            Debug.DrawRay(rayOrigin, rayDirection * 10f, Color.yellow);
        }
    }


}
