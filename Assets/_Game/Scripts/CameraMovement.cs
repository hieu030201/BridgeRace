using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraMovement : MonoBehaviour
{
    public Transform player; // Tham chiếu đến đối tượng người chơi.

    public Vector3 offset = new Vector3(0f, 2f, -5f); // Độ lệch tương đối giữa camera và người chơi.

    public float smoothSpeed = 5f; // Độ mượt của việc theo dõi.



    private void LateUpdate()
    {
        if (player == null)
        {
            // Nếu không tìm thấy người chơi, không làm gì.
            return;
        }

        // Tính vị trí mục tiêu cho camera.
        Vector3 desiredPosition = player.position + offset;

        // Sử dụng SmoothDamp để làm cho chuyển động của camera mượt hơn.
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // Camera luôn nhìn về người chơi.
        transform.LookAt(player);
    }

}

