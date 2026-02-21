using UnityEngine;

public class RoarPulseExpand : MonoBehaviour
{
    [SerializeField] private float expandSpeed = 15f;
    [SerializeField] private float maxScale = 12f;
    [SerializeField] private float pulseDelay = 0.025f;
    [SerializeField] private float lifeTime = 1f;

    private Vector3 originalScale;
    private bool expanding = true;

    

    void Start()
    {
        originalScale = transform.localScale;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (expanding)
        {
            transform.localScale += expandSpeed * Time.unscaledDeltaTime * Vector3.one;

            if (transform.localScale.x >= maxScale)
            {
                expanding = false;
                Invoke(nameof(ResetScale), pulseDelay);
            }
        }
    }

    void ResetScale()
    {
        transform.localScale = originalScale;
        expanding = true;
    }
}
