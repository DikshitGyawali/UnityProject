using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class MeleeWeapon : MonoBehaviour, IPausable
{
    [SerializeField] private int damageAmount = 5;
    [SerializeField] private PlayerHealth playerHealth;
    private PlayerState state;
    private Rigidbody2D rb;
    private Vector2 knockBackDirection;
    private readonly float knockbackTime= 0.1f;
    private readonly float attackInvulnarebleTime = 0.1f;
    public bool isDownwardStrike;
    [SerializeField] private bool isUpwardStrike;
    private bool collidedwithObj = false;
    private int upwardForce = 11;
    public int defaultForce = 10;

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
        state = GetComponentInParent<PlayerState>();
        rb = GetComponentInParent<Rigidbody2D>();
        isDownwardStrike = false;
        isUpwardStrike = false;
    }
    private void FixedUpdate()
    {
        if (paused) return;
        if (state.IsDashing) return;
        HandleMovement();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Attackable>())
        {
            HandleCollision(collision.GetComponent<Attackable>());
        }
    }

    private void HandleCollision(Attackable obj)
    {
        if (obj.givesUpwardForce && (state.UpDownValue < -0.5f) && !state.IsGrounded && !state.CanAttack)
        {
            isDownwardStrike = true;
            upwardForce = obj.upwardForce;
            collidedwithObj = true;
        }
        else if (state.UpDownValue > 0.5f && !state.CanAttack)
        {
            isUpwardStrike = true;
            collidedwithObj = true;
        }
        else if (state.UpDownValue == 0 && !state.CanAttack)
        {
            if (state.IsFashingRight)  knockBackDirection = Vector2.left;
            else knockBackDirection = Vector2.right;
            collidedwithObj = true;
        }
        obj.Damage(damageAmount);
        StartCoroutine(NoLongerCollided());
    }

    private void HandleMovement()
    {
        if (collidedwithObj)
        {
            if (isDownwardStrike)
            {
                rb.linearVelocityY = upwardForce;
            }
            else if (isUpwardStrike)
            {
                if (rb.linearVelocityY > 0) rb.linearVelocityY = 0;
            }
            else rb.linearVelocityX = (knockBackDirection * defaultForce).x;
        }
    }

    private IEnumerator NoLongerCollided()
    {
        playerHealth.isInvincible = true;
        yield return new WaitForSeconds(attackInvulnarebleTime);
        playerHealth.isInvincible = false;
        yield return new WaitForSeconds(knockbackTime-attackInvulnarebleTime);
        collidedwithObj = false;
        isDownwardStrike = false;
        isUpwardStrike = false;
    }
}
