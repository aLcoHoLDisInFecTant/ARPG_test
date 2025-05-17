using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroHUD : MonoBehaviour
{
    public StatusCtrl status; // 角色状态引用

    [Header("UI Components")]
    public Image HPGauge;
    public TextMeshProUGUI HPText;
    public Image ArmGauge;
    public TextMeshProUGUI ArmText;

    void Start()
    {
        // 自动寻找绑定对象（可选，如果你想拖动引用则注释此段）
        if (status == null) status = FindObjectOfType<StatusCtrl>();

        if (HPGauge == null || HPText == null || ArmGauge == null || ArmText == null)
        {
            Transform canvas = GameObject.Find("Canvas_HUD_Hero").transform;
            HPGauge = canvas.Find("HPGauge").GetComponent<Image>();
            HPText = canvas.Find("HPText").GetComponent<TextMeshProUGUI>();
            ArmGauge = canvas.Find("ArmGauge").GetComponent<Image>();
            ArmText = canvas.Find("ArmText").GetComponent<TextMeshProUGUI>();
        }
    }

    void Update()
    {
        if (status == null) return;

        // 更新血量显示
        float hpRatio = (float)status.currentHealth / status.maxHealth;
        HPGauge.fillAmount = Mathf.Clamp01(hpRatio);
        HPText.text = $"{status.currentHealth} / {status.maxHealth}";

        // 更新护甲显示
        float armRatio = status.maxArm > 0 ? (float)status.currentArm / status.maxArm : 0;
        ArmGauge.fillAmount = Mathf.Clamp01(armRatio);
        ArmText.text = $"{status.currentArm} / {status.maxArm}";
    }
}
