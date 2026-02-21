using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Transform))]
public class DamagePlayerWhenOverlap : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerHealth>())
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>();
            if (!player.isInvincible) player.TakeDamage(this.GetComponent<Transform>());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerStay2D(collision);
    }
}
