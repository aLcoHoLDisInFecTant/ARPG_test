using UnityEngine;

public class SkillCoin : MonoBehaviour
{
    private GameObject skillTreeCanvas;

    void Start()
    {
        // 方法一：根据名称查找
        skillTreeCanvas = GameObject.Find("Canvas_SkillTree");

        // 方法二（可选）：你也可以给Canvas设置Tag，使用以下方式查找
        // skillTreeCanvas = GameObject.FindGameObjectWithTag("SkillTree");

        if (skillTreeCanvas == null)
        {
            Debug.LogError("❌ Canvas_SkillTree not found! Please ensure it exists in the scene.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (skillTreeCanvas != null)
        {
            Time.timeScale = 0f;
            skillTreeCanvas.SetActive(true);
        }

        Destroy(gameObject);
    }
}
