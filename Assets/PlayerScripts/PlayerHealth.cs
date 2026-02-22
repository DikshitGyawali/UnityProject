using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AttackManager))]
[RequireComponent(typeof(OnDeath))]
public class PlayerHealth : MonoBehaviour, IPausable
{
    private Rigidbody2D player;
    private PlayerMovement playerMovement;
    private AttackManager playerAttackManager;
    private OnDeath onDeath;
    [SerializeField] private SpriteRenderer playerSprite;
    public int health;
    public int maxHealth=5;
    public bool isInvincible;
    public bool isTakingDamage;
    public bool isHealing;
    public int healAmount = 1;
    [SerializeField] private float hitFlashSpeed;
    public float invincibleTime = 1f;
    public float noControlTime = 0.5f;
    public float healTime = 2f;
    private bool paused = false;
    private readonly float lowTimeScaleTime =0.05f;
    private readonly float lowtimeScale = 0.25f;
    private readonly float recoilX = 7;
    private readonly float recoilY = 10;
    public event Action<int> OnHealthChanged;
    private void Awake()
    {
        player = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAttackManager = GetComponent<AttackManager>();
        playerSprite = GetComponent<SpriteRenderer>();
        health = maxHealth;
        onDeath = GetComponent<OnDeath>();
    }
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

    private void OnHeal()
    {
        // when pressed heal button become invinsible 
        // but cant move, if in air remain in air, 
        // dash will not reset
        if (isHealing || isTakingDamage || playerMovement.isDashing || !playerMovement.canControl) return;
        
        if (health >= maxHealth) return;
        if(healAmount+health > maxHealth)
            StartCoroutine(Heal(maxHealth - health));
        else
            StartCoroutine(Heal(healAmount));
    }

    public void TakeDamage(Transform obj)
    {
        health -= 1;
        OnHealthChanged?.Invoke(health);
        ScreenShakeController.Instance.Shake(.1f,.25f, 20f);
        if (health <= 0)
        {
            //run death animation: TODO
            //Destroy(gameObject);
            onDeath.OnDeathMenu();
            return;
        }
        //Recoil
        playerMovement.canControl = false;
        playerAttackManager.canAttack = false;
        if (obj.transform.position.x <= this.transform.position.x)
            player.linearVelocityX = recoilX;
        else
            player.linearVelocityX = -recoilX;
        
        player.linearVelocityY = recoilY;
        
        StartCoroutine(StopTakingDamage());

    }

    IEnumerator StopTakingDamage()
    {
        isInvincible = true;
        isTakingDamage = true;
        GameTimeController.Instance.SlowTime(lowtimeScale, lowTimeScaleTime);
        
        float regainControlDelay = Mathf.Max(0f, noControlTime - lowTimeScaleTime);
        yield return new WaitForSeconds(regainControlDelay);
        playerMovement.canControl = true;
        playerAttackManager.canAttack = true;

        float remainingInvincibleTime = Mathf.Max(0f, invincibleTime - noControlTime - lowTimeScaleTime);
        yield return new WaitForSeconds(remainingInvincibleTime);
        isTakingDamage = false;
        isInvincible = false;
    }
    private IEnumerator Heal(int healAmount)
    {
        isInvincible = true;
        isHealing = true;
        float originalGravityScale = player.gravityScale;
        player.gravityScale = 0;
        player.linearVelocity = new Vector2(0,0);
        playerMovement.canControl = false;

        yield return new WaitForSeconds(healTime);

        health += healAmount;
        OnHealthChanged?.Invoke(health);

        playerMovement.canControl = true;
        player.gravityScale = originalGravityScale;
        isHealing = false;
        isInvincible = false;
    }
    void Update()
    {
        if (paused) return;
        //needs to change I should use coroutine to flash the player when hit, but for now this is fine
        if (isInvincible){
            playerSprite.material.color = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time * hitFlashSpeed, 0.75f));
        }
        else
            playerSprite.material.color = Color.white;
    }   
}
