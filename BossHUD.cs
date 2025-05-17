using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHUD : MonoBehaviour
{
    public StatusCtrl status;

    [Header("UI Components")]
    public Image HPGauge;
    public TextMeshProUGUI HPText;

    void Start()
    {
        if (status == null) status = FindObjectOfType<StatusCtrl>();

        if (HPGauge == null || HPText == null)
        {
            Transform canvas = GameObject.Find("Canvas_HUD_Boss").transform;
            HPGauge = canvas.Find("HPGauge").GetComponent<Image>();
            HPText = canvas.Find("HPText").GetComponent<TextMeshProUGUI>();
        }
    }

    void Update()
    {
        if (status == null) return;

        float hpRatio = (float)status.currentHealth / status.maxHealth;
        HPGauge.fillAmount = Mathf.Clamp01(hpRatio);
        HPText.text = $"{status.currentHealth} / {status.maxHealth}";
    }
}
