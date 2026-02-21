using UnityEngine;

public class BossRoar : MonoBehaviour
{
    [SerializeField] private Transform headPoint;

    [Header("Roar Sprites")]
    [SerializeField] private GameObject repeatingExpandPrefab;

    public void TriggerRoar()
    {
        Instantiate(repeatingExpandPrefab, headPoint.position, Quaternion.identity);
    }
}
