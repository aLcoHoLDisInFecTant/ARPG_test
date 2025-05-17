using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject canvasMainMenu;
    public GameObject canvasHUD;
    public GameObject playerPrefab;
    public Transform birthPoint;

    public void StartGame_Normal()
    {
        EventManager.Instance.SetGameMode(EventManager.GameMode.Normal);
        StartGame();
    }

    public void StartGame_Training()
    {
        EventManager.Instance.SetGameMode(EventManager.GameMode.Training);
        StartGame();
    }

    private void StartGame()
    {
        canvasMainMenu.SetActive(false);
        canvasHUD.SetActive(true);

        GameObject player = Instantiate(playerPrefab, birthPoint.position, birthPoint.rotation);
    }
}
