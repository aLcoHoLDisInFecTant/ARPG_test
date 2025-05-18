using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    //public GameObject canvasMainMenu;
    public GameObject Canvas_SkillTree;
    public GameObject Canvas_HUD_Hero;
    public GameObject Canvas_HUD_Boss;
    public GameObject playerPrefab;
    public Transform birthPoint;

    private void Start()
    {
        StartGame();
    }
    

    private void StartGame()
    {
        Canvas_SkillTree.SetActive(false);
        Canvas_HUD_Hero.SetActive(true);
        Canvas_HUD_Boss.SetActive(true);

        GameObject player = Instantiate(playerPrefab, birthPoint.position, birthPoint.rotation);
    }
    
}
