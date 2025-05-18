using UnityEngine;
using System.Collections;



public class FireballSkillEffect : MonoBehaviour, ISkillEffect
{
    public int fireballLevel = 1;

    public void Activate(GameObject caster, GameObject target)
    {
        var status = target.GetComponent<StatusCtrl>();
        if (status == null)
        {
            Debug.LogWarning("❌ Fireball target has no StatusCtrl!");
            return;
        }

        switch (fireballLevel)
        {
            case 1:
                status.TakeDamage(30, caster.transform);
                break;
            case 2:
                StartCoroutine(DamageOverTime(status, 2f));
                break;
            case 3:
                StartCoroutine(DamageOverTime(status, 3f));
                break;
        }

        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
        if (ps != null)
            Destroy(gameObject, ps.main.duration);
        else
            Destroy(gameObject, 2f); // fallback
    }

    private IEnumerator DamageOverTime(StatusCtrl status, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            status.TakeDamage(10, transform); // 来自自身即可
            yield return new WaitForSeconds(0.5f);
            elapsed += 0.5f;
        }
    }
}
