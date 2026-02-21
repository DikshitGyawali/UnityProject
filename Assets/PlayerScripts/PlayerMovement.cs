using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerState))]
[RequireComponent(typeof(TrailRenderer))]
public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    private PlayerState state;
    private TrailRenderer tr;

    public bool canControl = true;
    [Header("Walking Settings")]
    public float walkSpeed;
    public float x_axis;
    public bool facingRight = true;
    [Header("Dash Settings")]
    public bool isDashing;
    [SerializeField] private bool canDash = true;
    [SerializeField] private float dashingPower = 65f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = .175f;

    [Header("Jump Settings")]
    public float jumpSpeed;
    [SerializeField] private int jumpBufferFrames = 8;
    [SerializeField] private float cayoteTime = 0.075f;
    private int jumpBufferCounter = 0;
    private float cayoteTimeCounter = 0;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        state = GetComponent<PlayerState>();
        tr = GetComponent<TrailRenderer>();
        canControl = true;
    }
    void OnDash()
    {
        if (canDash && canControl)
            StartCoroutine(Dash());
    }
    void OnJump(InputValue inputValue)
    {
        if(!canControl) return;

        if (rb.linearVelocityY > 0) rb.linearVelocityY = 0;
        if(inputValue.Get<float>() == 1)
        {
            jumpBufferCounter = jumpBufferFrames;
        }
    }
    
    void OnMove(InputValue inputValue)
    {
        x_axis = inputValue.Get<float>();
        if ((x_axis > 0 && !facingRight) || (x_axis < 0 && facingRight)) // Moving in one direction and currently facing another
        {
            FlipCharacter();
        }
    }

    void Update()
    {
        if (isDashing) return;
        if (rb.linearVelocityY < -30) rb.linearVelocityY = -30;
        Move();
        Jump();
    }

    private IEnumerator Dash()
    {
        bool wasGrounded;
        if (state.IsGrounded) wasGrounded = true;
        else wasGrounded = false;
        state.CanAttack = false;
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.linearVelocityY = 0;
        rb.gravityScale = 0;
        tr.emitting = true;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        tr.emitting = false;
        isDashing = false;
        state.CanAttack = true;
        yield return new WaitForSeconds(dashingCooldown);
        if (!wasGrounded) yield return new WaitUntil(() => state.IsGrounded || state.IsDownwardStrike);
        
        canDash = true;
    }
    private void Move()
    {
        if(canControl){
            rb.linearVelocityX = x_axis * walkSpeed;
        }
    }
    private void Jump()
    {
        if (state.IsGrounded) cayoteTimeCounter = cayoteTime;
        else cayoteTimeCounter -= Time.deltaTime;

        if (jumpBufferCounter > 0 && cayoteTimeCounter > 0) rb.linearVelocityY = jumpSpeed;

        jumpBufferCounter--;
    }

    private void FlipCharacter()
    {
        facingRight = !facingRight;
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }
}
