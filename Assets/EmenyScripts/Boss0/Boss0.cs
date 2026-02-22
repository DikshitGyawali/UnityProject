using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EyeManipulation))]
public class Boss0 : Attackable
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private SpriteRenderer head;
    [SerializeField] private SpriteRenderer eye;
    [SerializeField] private SpriteRenderer pupil;
    [SerializeField] private GameObject rockPrefab;
    [SerializeField] private BoxCollider2D cameraBoundry;



    [SerializeField] private GameObject winMenu;




    private Animator anim;

    private enum BossState
    {
        Starting,
        Idle,
        Attacking,
        Dead
    }

    private BossState currentState = BossState.Starting;

    public enum PlayerZone
    {
        Left,
        Center,
        Right
    }

    public bool isEnraged;

    private Coroutine aiRoutine;

    #region Unity Lifecycle

    protected override void Start()
    {
        base.Start();
        winMenu.SetActive(false);
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        OnHealthChanged += HealthChanged;
        aiRoutine = StartCoroutine(AILoop());
    }

    private void OnDisable()
    {
        OnHealthChanged -= HealthChanged;

        if (aiRoutine != null)
            StopCoroutine(aiRoutine);
    }

    #endregion

    #region AI LOOP

    private IEnumerator AILoop()
    {
        while (currentState != BossState.Dead)
        {
            yield return new WaitUntil(() => currentState == BossState.Idle);

            // Small random delay between attacks
            yield return new WaitForSeconds(Random.Range(0.3f, 1f));

            ChooseAndStartAttack();
            yield return new WaitUntil(() => currentState == BossState.Idle);
            if (isEnraged)
                yield return StartCoroutine(RockSpawn());
        }
    }

    private void ChooseAndStartAttack()
    {
        currentState = BossState.Attacking;

        PlayerZone zone = DetectPlayerZone();

        if (!isEnraged)
            TriggerNormalAttack(zone);
        else
            TriggerEnragedAttack(zone);
    }

    #endregion

    #region Attack Selection

    private PlayerZone DetectPlayerZone()
    {
        float diff = player.position.x - transform.position.x;

        if (Mathf.Abs(diff) <= 5f)
            return PlayerZone.Center;
        else if (diff < 0)
            return PlayerZone.Left;
        else
            return PlayerZone.Right;
    }

    private void TriggerNormalAttack(PlayerZone zone)
    {
        switch (zone)
        {
            case PlayerZone.Center:
                anim.SetTrigger(Random.value < 0.5f ? "HandSwipe" : "HandSwipeRight");
                break;

            case PlayerZone.Left:
                anim.SetTrigger("AreaWideAttackLeft");
                break;

            case PlayerZone.Right:
                anim.SetTrigger("AreaWideAttackRight");
                break;
        }
    }

    private void TriggerEnragedAttack(PlayerZone zone)
    {
        switch (zone)
        {
            case PlayerZone.Center:
                anim.SetTrigger(Random.value < 0.5f ?
                    "EnragedHandSwipeLeft" :
                    "EnragedHandSwipeRight");
                break;

            case PlayerZone.Left:
                anim.SetTrigger("EnragedAreaWideAttackLeft");
                break;

            case PlayerZone.Right:
                anim.SetTrigger("EnragedAreaWideAttackRight");
                break;
        }
    }

    #endregion

    #region Animation Event Callback

    // Animation Event at end of every attack clip
    public void OnAttackAnimationFinished()
    {
        currentState = BossState.Idle;
    }
    public void BossScream()
    {
        GameTimeController.Instance.SlowTime(.2f, 1);

    }
    public void CameraShake()
    {
        ScreenShakeController.Instance.Shake(.2f,.6f, 15f);
    }
    public void ChangeCameraBoundry(int i)
    {
            bool extend = (i == 1);
        if (extend)
        {
            cameraBoundry.offset += new Vector2(0, 2f);
            cameraBoundry.size += new Vector2(0, 4f);
        }
        else
        {
            cameraBoundry.offset -= new Vector2(0, 2f);
            cameraBoundry.size -= new Vector2(0, 4f);
        }
    }
    #endregion

    #region Health & Phase Handling

    private void HealthChanged(int currentHealth)
    {
        if (currentHealth <= health / 1.5 && !isEnraged)
        {
            isEnraged = true;
            anim.SetTrigger("Enraged");
        }

        if (currentHealth <= 0)
        {
            currentState = BossState.Dead;
            ChangeCameraBoundry(0);
            winMenu.SetActive(true);
            Destroy(gameObject);
        }
    }

    protected override IEnumerator BeingHit()
    {
        Color prevHead = head.color;
        Color prevEye = eye.color;
        Color prevPupil = pupil.color;

        head.color = new Color(1f, 1f, 1f, .7f);
        eye.color = new Color(1f, 1f, 1f, .5f);
        pupil.color = new Color(1f, 1f, 1f, .7f);

        yield return new WaitForSeconds(invulnarebleTime);

        head.color = prevHead;
        eye.color = prevEye;
        pupil.color = prevPupil;

        canBeHit = true;
    }

    #endregion

    #region Enraged Rock Spawn

    private IEnumerator RockSpawn()
    {
        float yOffset = 6f;

        // First rock above player
        SpawnRock(player.position.x, yOffset);

        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(Random.value * 1.4f);

            float randomX = Random.Range(-9f, 9f);
            SpawnRock(player.position.x + randomX, yOffset);
        }

        yield return new WaitForSeconds(1.1f);
    }

    private void SpawnRock(float xPosition, float yOffset)
    {
        float zRotation = Random.Range(0f, 90f);
        Quaternion rotation = Quaternion.Euler(0, 0, zRotation);

        Instantiate(
            rockPrefab,
            new Vector3(xPosition, transform.position.y + yOffset, transform.position.z),
            rotation
        );
    }

    #endregion
}
