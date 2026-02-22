using UnityEngine;
using UnityEngine.InputSystem;

public class OnDeath : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject deathMenu;
    
    private void Start()
    {
        deathMenu.SetActive(false);
    }
    public void OnDeathMenu()
    {
        Debug.Log("Death Menu Button Pressed");
        GamePause.Toggle(playerInput);
        deathMenu.SetActive(true);
    }
    public void Retry()
    {
        Debug.Log("Retry Button Pressed");
        GamePause.Toggle(playerInput);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        GamePause.Toggle(playerInput);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
