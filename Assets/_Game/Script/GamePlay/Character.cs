
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

    private List<BrickSpawn> bricks = new();
   
    public ColorType ColorType { get => colorType; set => colorType = value; }
    public ColorDataSO ColorDataSO { get => colorDataSO; set => colorDataSO = value; }

    public void OnInit()
    {

    }

    public void OnDespawn()
    {
        Destroy(gameObject);
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

            bricks.Add(brick);

        }
        else
        {
            Debug.Log("Diff ColorType");
        }


    }

    public void RemoveBrick()
    {
        Debug.Log(bricks.Count);
        if (bricks.Count == 0) return;

        BrickSpawn lastBrick = bricks[bricks.Count - 1];
        bricks.RemoveAt(bricks.Count - 1);
        
        Destroy(lastBrick.gameObject);
        
        
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



}
