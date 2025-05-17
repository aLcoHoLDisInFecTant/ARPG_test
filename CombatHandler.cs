using UnityEngine;
using UnityEditor;

public class CombatHandler : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 10f;
    public int Combo1Damage = 10;
    public int Combo2Damage = 12;
    public int Combo3Damage = 15;
    public int Combo4Damage = 20;
    public int Skill_01Damage = 22;
    public int Skill_02Damage = 30;
    public int Skill_03Damage = 31;
    public LayerMask enemyLayers;

    public Animator animator;

    private int currentCombo = 0;
    private float comboTimer = 0;
    private float comboResetTime = 1.0f;
    public bool isAtk = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (currentCombo > 0)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer > comboResetTime)
            {
                ResetCombo();
            }
        }
    }

    public void PerformGesture(GestureType gesture)
    {
        
        isAtk = true;
        switch (gesture)
        {
            case GestureType.Click:
                animator.SetBool("Atk", true);
                HandleComboAttack();
                break;

            case GestureType.Slash_Up:
                Debug.Log("Skill 01 - Slash Up");
                PlayAnimation("Skill_01");
                ExecuteSkillAttack(Skill_01Damage, Vector3.up, 45f);
                break;

            case GestureType.Slash_Down:
                Debug.Log("Skill 02 - Slash Down");
                PlayAnimation("Skill_02");
                ExecuteSkillAttack(Skill_02Damage, Vector3.down, 45f);
                break;

            case GestureType.Slash_Z:
                Debug.Log("Skill 03 - Slash Z");
                PlayAnimation("Skill_03");
                ExecuteSkillAttack(Skill_03Damage, attackPoint.forward, 90f);
                break;

            default:
                Debug.LogWarning("Unrecognized gesture.");
                break;
        }
    }

    void HandleComboAttack()
    {
        currentCombo++;
        comboTimer = 0f;

        switch (currentCombo)
        {
            case 1:
                PlayAnimation("Combo1");
                ExecuteAttack(Combo1Damage);
                break;
            case 2:
                PlayAnimation("Combo2");
                ExecuteAttack(Combo2Damage);
                break;
            case 3:
                PlayAnimation("Combo3");
                ExecuteAttack(Combo3Damage);
                break;
            case 4:
                PlayAnimation("Combo4");
                ExecuteAttack(Combo4Damage);
                ResetCombo(); // combo4是最后一段，强制重置
                break;
            default:
                isAtk = false;
                ResetCombo();
                break;
        }
    }

    void ExecuteAttack(int damage)
    {
        Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);
        foreach (var hit in hits)
        {
            var stats = hit.GetComponent<StatusCtrl>();
            if (stats != null)
            {
                stats.TakeDamage(damage, transform);
            }
            SpawnHitEffect(hit.ClosestPoint(attackPoint.position));
        }
    }

    void ExecuteSkillAttack(int damage, Vector3 direction, float angle)
    {
        Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);
        foreach (var hit in hits)
        {
            Vector3 dirToTarget = (hit.transform.position - attackPoint.position).normalized;
            float angleToTarget = Vector3.Angle(direction, dirToTarget);
            if (angleToTarget < angle)
            {
                var stats = hit.GetComponent<StatusCtrl>();
                if (stats != null)
                {
                    stats.TakeDamage(damage, transform);
                }
                SpawnHitEffect(hit.ClosestPoint(attackPoint.position));
            }
        }
    }

    void PlayAnimation(string trigger)
    {
        if (animator != null)
        {
            Debug.Log("trigger" + trigger);
            animator.SetTrigger(trigger);
        }
    }

    void SpawnHitEffect(Vector3 position)
    {
        Debug.Log("Hit effect spawned at: " + position);
    }

    void ResetCombo()
    {
        animator.SetBool("Atk", false);
        currentCombo = 0;
        comboTimer = 0f;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }


}
