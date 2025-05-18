using UnityEngine;
using UnityEngine.UI;

public class SkillWindow : MonoBehaviour
{
    public HeroSkillSystem heroSkillSystem;

    public Button healButton;
    public Button fireballButton;

    void Start()
    {
        healButton.onClick.AddListener(() => OnSkillSelected(SkillType.Heal));
        fireballButton.onClick.AddListener(() => OnSkillSelected(SkillType.Fireball));
    }

    void OnSkillSelected(SkillType type)
    {
        if (heroSkillSystem == null) return;

        var state = heroSkillSystem.skills[type];
        if (!state.IsMaxed)
        {
            state.Upgrade();
            Debug.Log($"✅ Upgraded skill: {type} to level {state.level}");
        }
        else
        {
            Debug.Log($"⚠️ {type} already at max level.");
        }

        CloseWindow();
    }

    void CloseWindow()
    {
        Time.timeScale = 1f; // 恢复游戏速度
        this.gameObject.SetActive(false);
    }
}
