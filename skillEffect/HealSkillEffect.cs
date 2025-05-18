using UnityEngine;

public class HealSkillEffect : MonoBehaviour, ISkillEffect
{
    public int skillLevel = 1;

    public void Activate(GameObject caster, GameObject target)
    {
        var status = target.GetComponent<StatusCtrl>();
        if (status == null) return;

        switch (skillLevel)
        {
            case 1:
                status.currentHealth = Mathf.Min(status.maxHealth, status.currentHealth + 30);
                break;
            case 2:
                status.currentHealth = Mathf.Min(status.maxHealth, status.currentHealth + 50);
                status.currentArm += 20;
                break;
            case 3:
                status.currentHealth = status.maxHealth;
                status.currentArm = status.maxArm;
                break;
        }

        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
        if (ps != null)
            Destroy(gameObject, ps.main.duration);
        else
            Destroy(gameObject, 2f);
    }
}
