using UnityEngine;

public class Enemy01 : Enemy
{
    [SerializeField] protected Rigidbody2D rb;
    private readonly float groundCheckY = 0.05f;
    private readonly float groundCheckX = 0.25f;
    private readonly float frontX = 0.1f;
    [SerializeField] private LayerMask ground;
    [SerializeField] private Transform checkPoint;
    [SerializeField] private float walkSpeed;
    protected override void Start()
    {
        health = 15;
        canBeHit = true;

        base.Start();
    }

    void Update()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Destroyed");
        }
        if (!HasGroundInFront(checkPoint))
        {
            FlipCharacter();
        }

    }
    void FixedUpdate()
    {
        if (IsGrounded()){
            if(facingRight) rb.linearVelocityX = walkSpeed;
            else rb.linearVelocityX = -walkSpeed;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerHealth>())
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>();
            if (!player.isInvincible) Attack(player);
        }
    }
    private void Attack(PlayerHealth player)
    {
        player.TakeDamage(this.GetComponent<Transform>());
    }
    private bool HasGroundInFront(Transform _checkPoint)
    {
        if (facingRight) return Physics2D.Raycast(_checkPoint.position + new Vector3(groundCheckX + frontX, 0, 0),
                                                    Vector2.down, groundCheckY, ground);

        else return Physics2D.Raycast(_checkPoint.position - new Vector3(groundCheckX + frontX, 0, 0),
                                        Vector2.down, groundCheckY, ground);
    }
    public bool IsGrounded()
    {
        return Physics2D.Raycast(checkPoint.position, Vector2.down, groundCheckY, ground)
        || Physics2D.Raycast(checkPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, ground)
        || Physics2D.Raycast(checkPoint.position - new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, ground);
    }
}
