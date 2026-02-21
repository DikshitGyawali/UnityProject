using UnityEngine;

public class Rock : Attackable
{
    private float yStart;
    private bool hasTouchedGround;
    void OnEnable()
    {
        yStart = transform.position.y;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += 5 * Time.deltaTime * Vector3.down;
        if (transform.position.y < yStart - 20) Destroy(gameObject);
    }
    public override void Damage(int healthAmount)
    {
        
    }
}
