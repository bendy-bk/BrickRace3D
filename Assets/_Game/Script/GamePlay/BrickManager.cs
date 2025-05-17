using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickManager : GenericSingleton<BrickManager>
{
    [SerializeField] private Transform brickListRoot;
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private float brickHeight = 0.2f;

    private List<GameObject> bricks = new();

    public void AddBrick(GameObject brick)
    {
        int index = bricks.Count;
        float yOffset = index * brickHeight;

        GameObject newBrick = Instantiate(brickPrefab, brickListRoot);
        newBrick.transform.localPosition = new Vector3(0, yOffset, 0);

        var col = newBrick.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        bricks.Add(newBrick);

        Destroy(brick); // hoặc brick.SetActive(false);
    }

    public void RemoveBrick()
    {
        if (bricks.Count == 0) return;

        GameObject lastBrick = bricks[^1];
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




}
