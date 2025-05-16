using System.Buffers.Text;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class PlayerController : GenericSingleton<PlayerController>
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    private Vector2 startTouch;
    private Vector2 swipeDirection;
    private bool isTouching = false;

    [Header("Manager Brick")]
    //[SerializeField] private Transform playerVS;
    [SerializeField] private Transform brickListVS;
    [SerializeField] private GameObject brickPrefab;
    private float brickHeight = 0.2f;
    private List<GameObject> bricksList = new List<GameObject>();

    void Update()
    {
        HandleTouchInput();
        MovePlayer();
    }


    // Lấy thông tin vuốt màn hình từ người chơi
    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // chỉ lấy ngón tay đầu tiên

            if (touch.phase == TouchPhase.Began)
            {
                startTouch = touch.position;
                isTouching = true;
            }
            /*Mỗi frame khi tay bạn di chuyển, touch.position sẽ cập nhật.

            delta = currentTouch - startTouch sẽ là hướng mới tính từ lúc bắt đầu chạm → đến vị trí hiện tại.

            Nếu người chơi liên tục đổi hướng ngón tay mà không nhấc ra khỏi màn hình:

                delta sẽ thay đổi liên tục.

            Do đó, swipeDirection cũng sẽ thay đổi liên tục.

            => Nhân vật cũng sẽ đổi hướng theo luôn trong lúc vẫn giữ tay.*/
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                Vector2 currentTouch = touch.position;
                Vector2 delta = currentTouch - startTouch;

                // Tránh swipe quá nhỏ
                if (delta.magnitude > 20f)
                {
                    swipeDirection = delta.normalized;
                }
            }

            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isTouching = false;
                swipeDirection = Vector2.zero;
            }
        }
        else
        {
            isTouching = false;
            swipeDirection = Vector2.zero;
        }
    }
    void MovePlayer()
    {
        if (!isTouching || swipeDirection == Vector2.zero) return;

        // Lấy hướng camera (đã loại bỏ trục Y để giữ nhân vật không bay)
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // Chuyển swipeDirection (Vector2) sang hướng 3D dựa theo camera
        Vector3 moveDir = camRight * swipeDirection.x + camForward * swipeDirection.y;
        moveDir.Normalize();

        // Di chuyển nhân vật
        transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);

        // Xoay mặt nhân vật theo hướng di chuyển
        if (moveDir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);
        }
    }

    public void AddBrick(GameObject brick)
    {
        // Spawn một bản mới từ prefab và đặt vào brickList
        int index = bricksList.Count;
        float brickY = index * brickHeight;

        //Spawn right position
        GameObject newBrick = Instantiate(brickPrefab, brickListVS);
        newBrick.transform.localPosition = new Vector3(0, brickY, 0);

        // Tắt collider nếu có (để tránh va chạm không cần thiết)
        var col = newBrick.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // Thêm vào danh sách quản lý
        bricksList.Add(newBrick);

        // Optionally: xóa brick cũ ngoài scene nếu là object thu thập

        Destroy(brick); // hoặc deactivate: brick.SetActive(false);

        Debug.Log("Stack Count: " + bricksList.Count);
    }
    public void RemoveBrick()
    {

        if (bricksList.Count == 0) return;

        GameObject lastBrick = bricksList[bricksList.Count - 1];
        bricksList.RemoveAt(bricksList.Count - 1);

        Destroy(lastBrick);
      
    }

    public void ClearStack()
    {
        // Xóa toàn bộ con của brickParent
        foreach (Transform child in brickListVS)
        {
            Destroy(child.gameObject);
        }

        // Dọn danh sách
        bricksList.Clear();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Brick"))
        {
            Debug.Log("Call add");
            AddBrick(other.gameObject);
        }
        else if(other.CompareTag("GetBrick"))
        {
            other.GetComponent<MeshRenderer>().material = null;
        }


    }

}
