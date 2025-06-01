using System.Buffers.Text;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class PlayerController : Character
{
    [SerializeField] private GameObject player;
    [SerializeField] private LayerMask groundLayer;
    [Header("Movement")]
    public float moveSpeed = 5f;
    private Vector2 startTouch;
    private Vector2 swipeDirection;
    private bool isTouching = false;
    void Update()
    {
        HandleTouchInput();
        //RaycastCheckBrick(player.transform.position, EnpointRaycast.position);
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
        if (!isTouching || swipeDirection == Vector2.zero)
        {
            return;
        }
        // Hướng từ camera
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        Vector3 moveDir = (camRight * swipeDirection.x + camForward * swipeDirection.y).normalized;
        Vector3 finalMove = moveDir;

        //Raycast
        Vector3 rayOrigin = transform.position + Vector3.up * 1f;
        //Debug.Log(rayOrigin);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(rayOrigin, Vector3.down, out hit, 100f, groundLayer);
        Debug.DrawLine(rayOrigin, rayOrigin + Vector3.down * 100f, Color.red);

        ///Check wall phia truoc
        if (ShouldBlockMovement(transform.position, moveDir))
        {
            Debug.Log("Movement blocked by Wall");
            return;
        }
        //if (IsWallInFront(finalMove))
        //{
        //    Debug.Log("Blocked by Wall");
        //    return;
        //}

        if (hasHit)
        {
            Vector3 groundNormal = hit.normal;
            finalMove = Vector3.ProjectOnPlane(moveDir, groundNormal).normalized;
            // Di chuyển bằng MoveTowards
            float moveSpeed = 5f; // hoặc public float moveSpeed;
            Vector3 targetPosition = transform.position + finalMove * moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }

        //Player luon dung thang khi len doc
        player.transform.rotation = Quaternion.identity;
    }

    public override void OnInit()
    {
        base.OnInit();
    }

    public override void OnDespawn(GameObject g)
    {
        base.OnDespawn(g);
    }
}

