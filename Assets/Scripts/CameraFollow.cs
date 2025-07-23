using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private static Transform target;
    public Vector3 offset = new Vector3(0, 8, -8);
    public float smoothSpeed = 0.125f;

    public static void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

    }
}
