using System;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;
public static class GamePause
{
    public static bool IsPaused { get; private set; } = false;
    public static event Action OnPaused;
    public static event Action OnResumed;

    public static void Pause(PlayerInput playerInput)
    {
        if (IsPaused) return;

        IsPaused = true;
        Time.timeScale = 0f;
        playerInput.SwitchCurrentActionMap("UI");
        Debug.Log("Game Paused");
        OnPaused?.Invoke();
    }

    public static void Resume(PlayerInput playerInput)
    {
        if (!IsPaused) return;

        IsPaused = false;
        Time.timeScale = 1f;
        playerInput.SwitchCurrentActionMap("Player");
        Debug.Log("Game Resumed");
        OnResumed?.Invoke();
    }

    public static void Toggle(PlayerInput playerInput)
    {
        if (IsPaused) Resume(playerInput);
        else Pause(playerInput);
    }
}
