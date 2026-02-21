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
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = baseFixedDeltaTime * Time.timeScale; // Important for physics consistency

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}
