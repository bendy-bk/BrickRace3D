using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Transform enpointRaycast;
    [SerializeField] private bool isBlock;

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
    protected Transform EnpointRaycast { get => enpointRaycast; set => enpointRaycast = value; }

    public virtual void OnInit()
    {

    }

    public virtual void OnDespawn(GameObject g)
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

        //Debug.Log($"Brick: {brick.BrickColorType}");
        //Debug.Log($"Player: {ColorType}");


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

        if (GetSBricksCount() == 0) return;

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

    public int GetSBricksCount() => bricks.Count;

    public bool ShouldBlockMovement(Vector3 rayOrigin, Vector3 moveDir)
    {
        RaycastHit hit;
        Vector3 origin = rayOrigin + Vector3.up * -0.3f;
        float distance = 1f;

        if (Physics.Raycast(origin, moveDir, out hit, distance))
        {
            Debug.DrawLine(origin, moveDir, Color.yellow);
            var brickStair = hit.collider.GetComponent<BrickStair>();
            if (brickStair != null)
            {
                // List Bricks on Player
                bool listCountBricks = GetSBricksCount() <= 0;
                // Compare ColorPlayer & brickStair
                bool compareColor = brickStair.BrickColorType.Equals(colorType);

                // bricks <= 0
                if (listCountBricks)
                {
                    // Color right || color = None
                    if (compareColor)
                    {
                        brickStair.Wall.SetActive(false);
                        return false;
                    }
                    else
                    {
                        brickStair.Wall.SetActive(true);
                        return true;
                    }

                }
                else 
                {
                    brickStair.Wall.SetActive(false);
                    return false;
                }
            }
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Brick"))
        {
            BrickSpawn brick = other.GetComponent<BrickSpawn>();

            if (brick != null && brick.BrickColorType == ColorType)
            {
                AddBrick(brick);
            }
        }
        else if (other.CompareTag("GetBrick"))
        {
            BrickStair brickStair = other.GetComponent<BrickStair>();
            MeshRenderer mes = other.GetComponent<MeshRenderer>();

            bool shouldChangeColor = GetSBricksCount() > 0;

            Debug.Log(shouldChangeColor);
            Debug.Log(brickStair.BrickColorType);
            if (shouldChangeColor)
            {
                if (brickStair.BrickColorType == ColorType)
                {
                    return;
                }
                else
                {
                    Debug.Log("Changed");
                    mes.material = ColorDataSO.GetMaterial(ColorType);
                    brickStair.ChagerColorType(ColorType);
                    RemoveBrick();
                }
            }
            else
            {
                if (brickStair.BrickColorType == ColorType)
                {
                    return;
                }
            }

        }

    }

}
