using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUICtrl : MonoBehaviour
{
    public Button startBtn;
    public Button trainBtn;

    private void Start()
    {
        startBtn.onClick.AddListener(() =>
        {
            EventManager.Instance.SetGameMode(EventManager.GameMode.Normal);
            SceneManager.LoadScene("MainScene");
        });

        trainBtn.onClick.AddListener(() =>
        {
            EventManager.Instance.SetGameMode(EventManager.GameMode.Training);
            SceneManager.LoadScene("MainScene");
        });
    }
}
