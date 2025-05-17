using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroHUD : MonoBehaviour
{
    public StatusCtrl status; // ��ɫ״̬����

    [Header("UI Components")]
    public Image HPGauge;
    public TextMeshProUGUI HPText;
    public Image ArmGauge;
    public TextMeshProUGUI ArmText;

    void Start()
    {
        // �Զ�Ѱ�Ұ󶨶��󣨿�ѡ����������϶�������ע�ʹ˶Σ�
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

        // ����Ѫ����ʾ
        float hpRatio = (float)status.currentHealth / status.maxHealth;
        HPGauge.fillAmount = Mathf.Clamp01(hpRatio);
        HPText.text = $"{status.currentHealth} / {status.maxHealth}";

        // ���»�����ʾ
        float armRatio = status.maxArm > 0 ? (float)status.currentArm / status.maxArm : 0;
        ArmGauge.fillAmount = Mathf.Clamp01(armRatio);
        ArmText.text = $"{status.currentArm} / {status.maxArm}";
    }
}
