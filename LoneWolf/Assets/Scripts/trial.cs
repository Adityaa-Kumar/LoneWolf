using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Header("Target to Follow")]
    [Tooltip("The object (like sheep) that the camera will follow")]
    [SerializeField] private Transform target;

    [Header("Camera Settings")]
    [Tooltip("How quickly the camera catches up to the target")]
    [SerializeField] private float smoothSpeed = 5f;

    [Tooltip("Offset from the target position")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
