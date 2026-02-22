using UnityEngine;

public class Rock : Attackable, IPausable
{
    private float yStart;
    private bool hasTouchedGround;
    private bool paused = false;

    void OnEnable()
    {
        GamePause.OnPaused += OnPause;
        GamePause.OnResumed += OnResume;
        yStart = transform.position.y;
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

    // Update is called once per frame
    void Update()
    {
        if (paused) return;
        transform.position += 5 * Time.deltaTime * Vector3.down;
        if (transform.position.y < yStart - 20) Destroy(gameObject);
    }
    public override void Damage(int healthAmount)
    {
        //To do Here ::ASdfasdf{]
    }
}
