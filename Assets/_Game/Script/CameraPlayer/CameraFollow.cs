using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target; // Nhân vật (Player)

    [Header("Offset")]
    public Vector3 offset = new Vector3(0, 5f, -7f); // Độ lệch so với Player

    [Header("Smooth Follow")]
    public float followSpeed = 5f;

    private void LateUpdate()
    {
        if (target == null) return;

        // Vị trí mong muốn của camera
        Vector3 desiredPosition = target.position + offset;

        // Di chuyển mượt đến vị trí mong muốn
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // (Tuỳ chọn) Luôn nhìn vào nhân vật
        transform.LookAt(target);
    }
}
