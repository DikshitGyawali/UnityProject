using System;
using UnityEngine;

public class CameraController : MonoBehaviour, IPausable
{
    [SerializeField] private Transform playerCameraCenter;
    [SerializeField] private Collider2D confiner;
    [SerializeField] private float followSpeed = 10f;
    [SerializeField] private Vector3 offsetZ;

    [SerializeField] private Camera cam;
    float halfWidth, halfHeight;
    
    private bool paused = false;

    void OnEnable()
    {
        GamePause.OnPaused += OnPause;
        GamePause.OnResumed += OnResume;
    }

    void OnDisable()
    {
        GamePause.OnPaused -= OnPause;
        GamePause.OnResumed -= OnResume;
    }
    public void OnPause()
    {
        paused = true;
    }
    public void OnResume()
    {
        paused = false;
    }
    
    void Awake()
    {
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;
    }
    void LateUpdate()
    {
        if (paused) return;
        if (!playerCameraCenter) return;

        if (!confiner)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                playerCameraCenter.position + offsetZ,
                followSpeed * Time.deltaTime
            );
            return;
        }

        Vector3 desired = playerCameraCenter.position;
        desired.z = transform.position.z;

        Vector2[] corners =
        {
            new(desired.x - halfWidth, desired.y - halfHeight),
            new(desired.x - halfWidth, desired.y + halfHeight),
            new(desired.x + halfWidth, desired.y - halfHeight),
            new(desired.x + halfWidth, desired.y + halfHeight)
        };

        float pushLeft = 0, pushRight = 0, pushUp = 0, pushDown = 0;

        foreach (var corner in corners)
        {
            Vector2 closest = confiner.ClosestPoint(corner);
            Vector2 delta = closest - corner;

            if (delta.x > 0) pushRight = Mathf.Max(pushRight, delta.x);
            if (delta.x < 0) pushLeft  = Mathf.Min(pushLeft,  delta.x);

            if (delta.y > 0) pushUp    = Mathf.Max(pushUp,    delta.y);
            if (delta.y < 0) pushDown  = Mathf.Min(pushDown,  delta.y);
        }

        // Resolve X
        if (Math.Abs(pushRight) > Math.Abs(pushLeft)) desired.x += pushRight;
        else if (Math.Abs(pushRight) < Math.Abs(pushLeft)) desired.x += pushLeft;

        // Resolve Y
        if (Math.Abs(pushUp) > Math.Abs(pushDown)) desired.y += pushUp;
        else if (Math.Abs(pushUp) < Math.Abs(pushDown)) desired.y += pushDown;

        transform.position = Vector3.Lerp(
            transform.position,
            desired,
            followSpeed * Time.deltaTime
        );
    }
}
