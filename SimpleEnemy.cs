using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SimpleEnemy : MonoBehaviour
{
    public int maxHealth = 50;
    private int currentHealth;

    public Animator animator; // 可选，如果你希望有受击或死亡动画
    public bool destroyOnDeath = true;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount, Transform attacker)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage from {attacker.name}. HP left: {currentHealth}");

        if (animator)
            animator.SetTrigger("Hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} died.");

        if (animator)
            animator.SetTrigger("Die");

        if (destroyOnDeath)
        {
            Destroy(gameObject, 1.5f); // 延迟销毁允许动画播放
        }
        else
        {
            GetComponent<Collider>().enabled = false;
            this.enabled = false;
        }
    }

    // 供 CombatHandler 调用
    public void ReceiveHit(int damage, Transform source)
    {
        TakeDamage(damage, source);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 1f, 0.5f);
    }
}
