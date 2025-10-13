using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;      // The player’s transform
    public float smoothSpeed = 0.125f;
    public Vector3 offset;        // Optional: offset from player

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
        }
    }
}