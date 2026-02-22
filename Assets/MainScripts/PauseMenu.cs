using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject pauseMenu;
    private bool ignoreFirstPress = true;
    private void Start()
    {
        pauseMenu.SetActive(false);
    }
    public void OnTogglepause()
    {
        if (ignoreFirstPress)
        {
            ignoreFirstPress = false;
            Debug.Log("Ignored first double trigger");
            return;
        }
        Debug.Log("Toggle Pause Button Pressed");
        GamePause.Toggle(playerInput);
        Debug.Log("Pause Toggled");
        pauseMenu.SetActive(GamePause.IsPaused);
    }

    public void MainMenu()
    {
        GamePause.Toggle(playerInput);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
