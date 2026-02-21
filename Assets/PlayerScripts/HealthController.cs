using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class HealthController : MonoBehaviour
{

    [SerializeField] private PlayerHealth player;
    [SerializeField] private GameObject healthContainerPrefab;
    public Transform parentHealth;
    private Image[] healthFills;
    private GameObject[] healthContainers;


    void Awake()
    {
        healthContainers = new GameObject[player.maxHealth];
        healthFills = new Image[player.maxHealth];
        InstantiateHealth();
        UpdateHealthHUD(player.health);
    }
    void OnEnable()
    {
        player.OnHealthChanged += UpdateHealthHUD;
        // if ever OnMaxHealthChanged is created 
        // +=InstiatiateHealth();
    }

    void OnDisable()
    {
        player.OnHealthChanged -= UpdateHealthHUD;
        // if ever OnMaxHealthChanged is created 
        // -=InstiatiateHealth();
    }

    void InstantiateHealth()
    {
        ClearHealthUI();
        healthContainers = new GameObject[player.maxHealth];
        healthFills = new Image[player.maxHealth];
        for (int i = 0; i < player.maxHealth; ++i)
        {
            GameObject temp = Instantiate(healthContainerPrefab);
            temp.transform.SetParent(parentHealth, false);
            healthContainers[i] = temp;
            healthFills[i] = temp.transform.GetChild(0).GetComponent<Image>();
            healthContainers[i].SetActive(true);
        }
    }
    void UpdateHealthHUD(int currentHealth)
    {
        for (int i = 0; i < player.maxHealth; ++i)
        {
            if (i < currentHealth)
                healthFills[i].fillAmount = 1;
            else
            {
                healthFills[i].fillAmount = 0;
            }
        }
    }

    private void ClearHealthUI()
    {
        if (healthContainers == null) return;
        if (healthFills == null) return;

        for (int i = 0; i < healthContainers.Length; i++)
        {
            if (healthContainers[i] != null)
                Destroy(healthContainers[i]);
            if(healthFills[i] != null)
                Destroy(healthFills[i]);
        }
    }
}
