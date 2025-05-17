using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    public enum GameMode { Normal, Training }
    public GameMode CurrentMode { get; private set; } = GameMode.Normal;

    public event Action<GameMode> OnGameModeSet;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetGameMode(GameMode mode)
    {
        CurrentMode = mode;
        OnGameModeSet?.Invoke(mode);
    }
}
