using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 0, -10);
    public float smoothReturnSpeed = 5.0f;
    private Vector3 dragOrigin;
    private bool isDragging = false;
    private bool isReturning = false;

    void Update()
    {
        // Start dragging
        if (Input.GetMouseButtonDown(2))
        {
            isDragging = true;
            isReturning = false;
            dragOrigin = Input.mousePosition;
        }

        // Drag camera
        if (Input.GetMouseButton(2) && isDragging)
        {
            Vector3 currentMousePos = Input.mousePosition;
            Vector3 worldOrigin = Camera.main.ScreenToWorldPoint(dragOrigin);
            Vector3 worldCurrent = Camera.main.ScreenToWorldPoint(currentMousePos);
            Vector3 difference = worldCurrent - worldOrigin;

            transform.position -= new Vector3(difference.x, difference.y, 0);
            dragOrigin = currentMousePos;
            return;
        }

        // End dragging, start return
        if (Input.GetMouseButtonUp(2))
        {
            isDragging = false;
            isReturning = true;
        }

        // Snap-back after drag
        if (isReturning && player != null)
        {
            Vector3 targetPosition = player.position + offset;
            transform.position = Vector3.Lerp(
                transform.position,
                targetPosition,
                Time.deltaTime * smoothReturnSpeed
            );

            // Close enough—snap to target and end returning
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                transform.position = targetPosition;
                isReturning = false;
            }
            return;
        }

        // Normal follow (no smooth, no lag)
        if (!isDragging && !isReturning && player != null)
        {
            transform.position = player.position + offset;
        }
    }
}
