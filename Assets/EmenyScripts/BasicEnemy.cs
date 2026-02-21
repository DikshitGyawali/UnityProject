using UnityEngine;

public class Enemy : Attackable
{
    public bool facingRight = false;

    protected void FlipCharacter()
    {
        facingRight = !facingRight;
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }


}
