using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public Rigidbody2D rb;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask ground;
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private AttackManager attackManager;
    [SerializeField] private MeleeWeapon melleWeapon;

    private float groundCheckY = 0.05f;
    private float groundCheckX = 0.25f;

    public bool IsGrounded
    {
        get{
        return Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, ground)
        || Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, ground)
        || Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, ground);
        }
    }
    public bool IsJumping
    {
        get{return rb.linearVelocityY > 0 && !IsGrounded;}
    }

    public bool IsFalling
    {
        get{return rb.linearVelocityY < 0 && !IsGrounded;}
    }

    public bool IsDashing
    {
        get{return movement.isDashing;}
    }
    public bool IsFashingRight
    {
        get{return movement.facingRight;}
    }

    public bool IsWalking
    {
        get{return Math.Abs(movement.x_axis) > 0;}
    }

    public bool CanAttack
    {
        get{return attackManager.canAttack;}
        set{attackManager.canAttack = value;}
    }

    public bool CanControl
    {
        get{return movement.canControl;}
        set{movement.canControl = value;}
    }
    public float UpDownValue
    {
        get{return attackManager.upDownValue;}
    }
    // public float TimeBetweenAttack
    // {
    //     get{return attackManager.timeBetweenAttack;}
    // }
    public bool IsDownwardStrike
    {
        get{return melleWeapon.isDownwardStrike;}
    }
}
