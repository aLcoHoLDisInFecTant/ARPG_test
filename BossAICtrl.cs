using UnityEngine;
using System.Collections;

public class BossAICtrl : MonoBehaviour
{
    public Transform heroTarget;
    public float detectionRadius = 20f;
    public LayerMask playerLayer;
    public float angleThresholdAtk01 = 15f;
    public float angleThresholdAtk02 = 30f;

    public Animator animator;
    public CombatHandler combatHandler;

    private float inFrontTimer = 0f;
    private float movingFrontTimer = 0f;
    private bool isAttacking = false;
    private bool isHeroInRange = false;

    private Vector3 lastHeroPos;
    private bool heroMoved = false;

    void Start()
    {
        combatHandler = GetComponent<CombatHandler>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (heroTarget == null || isAttacking) return;

        Vector3 dirToHero = heroTarget.position - transform.position;
        dirToHero.y = 0;
        float distance = dirToHero.magnitude;

        // ½øÈëÌ½²â·¶Î§
        if (distance <= detectionRadius)
        {
            isHeroInRange = true;

            // ÃæÏòHero£¨»ºÂý×ªÏò£©
            Quaternion lookRotation = Quaternion.LookRotation(dirToHero.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f);

            float angle = Vector3.Angle(transform.forward, dirToHero.normalized);

            heroMoved = (Vector3.Distance(heroTarget.position, lastHeroPos) > 0.1f);
            lastHeroPos = heroTarget.position;

            // ¹¥»÷01ÅÐ¶Ï
            if (angle < angleThresholdAtk01)
            {
                inFrontTimer += Time.deltaTime;
                if (inFrontTimer >= 1f)
                    StartCoroutine(PerformAttack("Atk_01", 5, 0.5f, 5, new Vector3(2f, 2f, 4f)));
            }
            else inFrontTimer = 0f;

            // ¹¥»÷02ÅÐ¶Ï
            if (angle < angleThresholdAtk02 && heroMoved)
            {
                movingFrontTimer += Time.deltaTime;
                if (movingFrontTimer >= 1f)
                    StartCoroutine(PerformAttack("Atk_02", 3, 0.5f, 5, new Vector3(4f, 2f, 6f)));
            }
            else movingFrontTimer = 0f;
        }
        else
        {
            // HeroÀë¿ªÌ½²â·¶Î§ ¡ú ´¥·¢Î²°Í¹¥»÷
            if (isHeroInRange)
            {
                isHeroInRange = false;
                StartCoroutine(HandleTailWhip(dirToHero));
            }
        }
    }

    IEnumerator PerformAttack(string animName, int damage, float interval, int repeat, Vector3 boxSize)
    {
        isAttacking = true;
        animator.SetTrigger(animName);
        yield return new WaitForSeconds(0.25f); // ÆðÊÖµÈ´ý

        for (int i = 0; i < repeat; i++)
        {
            DoBoxDamage(boxSize, damage);
            yield return new WaitForSeconds(interval);
        }

        isAttacking = false;
        inFrontTimer = 0f;
        movingFrontTimer = 0f;
    }

    IEnumerator HandleTailWhip(Vector3 dirToHero)
    {
        isAttacking = true;

        bool isRight = Vector3.Dot(transform.right, dirToHero.normalized) > 0;
        string anim = isRight ? "TailWhipLeft" : "TailWhipRight";
        animator.SetTrigger(anim);

        yield return new WaitForSeconds(0.5f); // ÆðÊÖµÈ´ý

        Vector3 boxOffset = isRight ? transform.right * 2f : -transform.right * 2f;
        DoBoxDamage(new Vector3(2f, 2f, 2f), 10, transform.position + boxOffset);

        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }

    void DoBoxDamage(Vector3 size, int damage, Vector3? overridePos = null)
    {
        Vector3 center = overridePos ?? (transform.position + transform.forward * (size.z / 2));
        Collider[] hits = Physics.OverlapBox(center, size / 2, transform.rotation, playerLayer);

        foreach (var hit in hits)
        {
            var status = hit.GetComponent<StatusCtrl>();
            if (status != null)
                status.TakeDamage(damage, transform);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
