using UnityEngine;
using System.Collections;

public class ScreenShakeController : MonoBehaviour
{
    public static ScreenShakeController Instance;

    private Vector3 originalPosition;
    private Coroutine shakeRoutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        originalPosition = transform.localPosition;
    }

    public void Shake(float duration, float magnitude, float frequency = 25f)
    {
        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(ShakeRoutine(duration, magnitude, frequency));
    }

    private IEnumerator ShakeRoutine(float duration, float magnitude, float frequency = 25f)
    {
        float elapsed = 0f;
        float randomOffset = Random.Range(0f, 100f); // Different shake pattern each time

        while (elapsed < duration)
        {
            float strength = magnitude * (1f - (elapsed / duration)); // Falloff
            
            // Perlin noise creates smooth, natural motion
            float offsetX = (Mathf.PerlinNoise(randomOffset, elapsed * frequency) - 0.5f) * 2f * strength;
            float offsetY = (Mathf.PerlinNoise(randomOffset + 100f, elapsed * frequency) - 0.5f) * .5f * strength;

            transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}

