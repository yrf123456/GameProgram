using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;

    public Tilemap tilemap;  // Reference to the ground Tilemap

    private Vector2 minPos;
    private Vector2 maxPos;
    private float halfHeight;
    private float halfWidth;

    void Start()
    {
        // Get camera dimensions
        Camera cam = Camera.main;
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;

        // Get the boundaries of the Tilemap
        Bounds bounds = tilemap.localBounds;

        minPos = new Vector2(bounds.min.x + halfWidth, bounds.min.y + halfHeight);
        maxPos = new Vector2(bounds.max.x - halfWidth, bounds.max.y - halfHeight);
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        // Clamp the camera within the boundaries
        float clampX = Mathf.Clamp(desiredPosition.x, minPos.x, maxPos.x);
        float clampY = Mathf.Clamp(desiredPosition.y, minPos.y, maxPos.y);

        Vector3 clamped = new Vector3(clampX, clampY, desiredPosition.z);
        Vector3 smoothed = Vector3.Lerp(transform.position, clamped, smoothSpeed);

        transform.position = smoothed;
    }
}
