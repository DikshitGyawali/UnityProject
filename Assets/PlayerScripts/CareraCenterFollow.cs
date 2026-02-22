using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerState))]
public class CareraCenterFollow : MonoBehaviour, IPausable
{
    private Rigidbody2D rb;
    private PlayerState state;
    [SerializeField] private Transform cameraCenter;
    private readonly float idleCameraOffset = 1.25f;
    private readonly float movingCameraOffset = -0.20f;
    private float walkingCounter = 0;
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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        state = GetComponent<PlayerState>();
    }
    void Update()
    {
        if (paused) return;
        float x_offset;
        if (state.IsWalking) walkingCounter += Time.deltaTime;
        if (state.IsFashingRight)
        {
            if ((state.IsWalking || state.IsDashing) && walkingCounter >= 10)
            {
                x_offset = movingCameraOffset;
                walkingCounter = 0;
            }
            else x_offset = idleCameraOffset;
        }
        else
        {
            if ((state.IsWalking || state.IsDashing) && walkingCounter >= 10)
            {
                x_offset = -movingCameraOffset;
                walkingCounter = 0;
            }
            else x_offset = -idleCameraOffset;
        }
        cameraCenter.position = Vector3.Lerp(rb.position, rb.position + new Vector2(x_offset,0), 3 * Time.deltaTime);

    }
}
