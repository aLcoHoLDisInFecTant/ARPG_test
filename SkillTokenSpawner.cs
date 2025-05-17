using UnityEngine;

public class SkillTokenSpawner : MonoBehaviour
{
    public GameObject skillTokenPrefab;
    public Transform bossTransform;
    private float lastThreshold = 1f;
    private StatusCtrl status;

    void Start()
    {
        status = GetComponent<StatusCtrl>();
    }

    void Update()
    {
        float hpRatio = status.currentHealth / status.maxHealth;
        if (hpRatio <= lastThreshold - 0.25f)
        {
            lastThreshold -= 0.25f;
            SpawnSkillToken();
        }
    }

    void SpawnSkillToken()
    {
        Vector3 spawnPos = bossTransform.position + Random.insideUnitSphere * 2f;
        spawnPos.y = bossTransform.position.y;
        Instantiate(skillTokenPrefab, spawnPos, Quaternion.identity);
    }
}
