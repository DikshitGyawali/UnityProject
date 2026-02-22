using UnityEngine;
using System.Collections;

public class GameTimeController : MonoBehaviour
{
    public static GameTimeController Instance;
    private Coroutine slowMotionRoutine;
    private float baseFixedDeltaTime;


    private void Awake()
    {
        
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        baseFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void SlowTime(float timeScale, float duration)
    {
        if (slowMotionRoutine != null)
            StopCoroutine(slowMotionRoutine);

        slowMotionRoutine = StartCoroutine(SlowTimeRoutine(timeScale, duration));
    }

    private IEnumerator SlowTimeRoutine(float timeScale, float duration)
    {
        ApplyTimeScale(timeScale);
        yield return WaitForUnpausedSeconds(duration);
        ApplyTimeScale(1f);
    }

    private void ApplyTimeScale(float scale)
    {
        // If paused, force 0 regardless
        float finalScale = GamePause.IsPaused ? 0f : scale;

        Time.timeScale = finalScale;
        Time.fixedDeltaTime = baseFixedDeltaTime * finalScale;
    }

    public IEnumerator WaitForUnpausedSeconds(float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            if (!GamePause.IsPaused)
                timer += Time.unscaledDeltaTime;

            yield return null;
        }
    }
}
