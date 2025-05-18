using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType { Heal, Fireball }

[System.Serializable]
public class SkillState
{
    public int level = 0;
    public bool IsMaxed => level >= 3;
    public void Upgrade()
    {
        if (!IsMaxed) level++;
    }
}

public class HeroSkillSystem : MonoBehaviour
{
    [Header("Fireball Prefabs (by Level)")]
    public GameObject fireballLevel1Prefab;
    public GameObject fireballLevel2Prefab;
    public GameObject fireballLevel3Prefab;

    [Header("Healing Prefab")]
    public GameObject healSkillPrefab;

    [Header("技能释放位置")]
    public Transform skillSpawnPoint;

    public Dictionary<SkillType, SkillState> skills = new();

    void Awake()
    {
        skills[SkillType.Heal] = new SkillState();
        skills[SkillType.Fireball] = new SkillState();
    }

    public void CastHeal()
    {
        int level = skills[SkillType.Heal].level;
        GameObject healObj = Instantiate(healSkillPrefab, transform.position, Quaternion.identity);
        if (healObj.TryGetComponent(out HealSkillEffect effect))
        {
            effect.skillLevel = level;
            effect.Activate(gameObject, gameObject); // 自己给自己回血
        }
    }

    public void CastFireball(GameObject targetEnemy)
    {
        int level = skills[SkillType.Fireball].level;
        GameObject prefab = level switch
        {
            1 => fireballLevel1Prefab,
            2 => fireballLevel2Prefab,
            3 => fireballLevel3Prefab,
            _ => fireballLevel1Prefab
        };

        if (prefab == null || targetEnemy == null) return;

        GameObject fireball = Instantiate(prefab, targetEnemy.transform.position, Quaternion.identity);
        if (fireball.TryGetComponent(out FireballSkillEffect effect))
        {
            effect.fireballLevel = level;
            effect.Activate(gameObject, targetEnemy);
        }
    }

    void HealPlayer() => Debug.Log("💚 Heal player immediately");
    void AddShield() => Debug.Log("🛡️ Add temporary shield");
    IEnumerator HealOverTime()
    {
        Debug.Log("💧 Start HoT (Heal over Time)");
        for (int i = 0; i < 5; i++)
        {
            Debug.Log("💧 Tick Heal");
            yield return new WaitForSeconds(1f);
        }
    }
}
