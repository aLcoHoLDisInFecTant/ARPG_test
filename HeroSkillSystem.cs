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
    public Dictionary<SkillType, SkillState> skills = new();
    public StatusCtrl statusCtrl;

    private void Awake()
    {
        skills[SkillType.Heal] = new SkillState();
        skills[SkillType.Fireball] = new SkillState();
        statusCtrl = GetComponent<StatusCtrl>();
    }

    public void UpgradeSkill(SkillType type)
    {
        if (skills.ContainsKey(type))
        {
            skills[type].Upgrade();
            Debug.Log($"Skill {type} upgraded to level {skills[type].level}");
        }
    }

    public void CastSkill(SkillType type)
    {
        if (!skills.ContainsKey(type)) return;

        int level = skills[type].level;
        Debug.Log($"Casting {type} at level {level}");

        switch (type)
        {
            case SkillType.Heal:
                if (level >= 1) HealPlayer();
                if (level >= 2) AddShield();
                if (level == 3) StartCoroutine(HealOverTime());
                break;

            case SkillType.Fireball:
                // 🔥 接口留空，等待后续扩展
                break;
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
