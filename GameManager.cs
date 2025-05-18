using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameMode { Normal, Training }
    public GameMode CurrentMode { get; private set; } = GameMode.Normal;

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
    }

    public bool IsTrainingMode() => CurrentMode == GameMode.Training;
}
