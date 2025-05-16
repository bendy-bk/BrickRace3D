using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    private Vector2 startTouch;
    private Vector2 swipeDirection;
    private bool isTouching = false;

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

}
