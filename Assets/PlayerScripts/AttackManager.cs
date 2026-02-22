using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerState))]
[RequireComponent(typeof(PlayerMovement))]
public class AttackManager : MonoBehaviour, IPausable
{
    [SerializeField] private Animator meleeAnimator; // The animator on the meleePrefab
    private Animator anim; // The Animator component on the player
    private PlayerState state;
    public bool attackPressed = true;
    public float upDownValue = 0f;
    public float timeBetweenAttack;
    public bool canAttack = true;
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
        anim = GetComponent<Animator>();
        state = GetComponent<PlayerState>();
    }
    void OnAttack(InputValue value)
    {
        if (value.Get<float>() == 1)
            attackPressed = true;
        else
            attackPressed = false;
    }
    void OnUpDown(InputValue value)
    {
        upDownValue = value.Get<float>();
    }
    void FixedUpdate()
    {
        if (paused) return;
        if (attackPressed && canAttack)
        {
            if (upDownValue > 0.5)
            {
                // call animation for upslash (player):TODO
                meleeAnimator.SetTrigger("UpSlash");
            }
            else if (upDownValue < -0.5 && !state.IsGrounded)
            {
                //call animation for downslash (player):TODO
                meleeAnimator.SetTrigger("DownSlash");
            }
            else
            {
                //call animation for frontslash (player):TODO
                meleeAnimator.SetTrigger("Slash");
            }
            attackPressed = false;
            StartCoroutine(ResetTimeSinceAttack());
        }
    }
    public IEnumerator ResetTimeSinceAttack()
    {
        canAttack = false;
        yield return new WaitForSeconds(timeBetweenAttack);
        canAttack = true;
    }
}
