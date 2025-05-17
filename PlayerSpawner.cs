using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform birthPoint;

    private void Start()
    {
        Instantiate(playerPrefab, birthPoint.position, birthPoint.rotation);
    }
}
