using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class StatusCtrl : MonoBehaviour
{
    // Start is called before the first frame update

    public int characterId = 0; 
    public int maxArm = 0;
    public int maxHealth = 200;
    public int currentArm;
    public int currentHealth;
    public Animator animator;
    public bool isFired = false;
    public float DebuffDuration = 3f;
    public float currentBuffDuration = 0;
    void Awake()
    {
        currentHealth = maxHealth;
        currentArm = maxArm;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage, Transform source) 
    {
        int finalDamage = Mathf.Max(damage, 1);
        Debug.Log($"{gameObject.name} took {damage} damage from {source.name}. HP left: {currentHealth}");
        if (currentArm == 0) currentHealth -= finalDamage;
        else if (currentArm > 0) 
        { 
            currentArm -= finalDamage;
            if (currentHealth < 0) { currentArm = 0; }
        }
        animator.SetTrigger("GotHit");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died.");
        animator.SetTrigger("Death");

    }


    void SetFired() 
    { 
        isFired = true;
        currentBuffDuration = DebuffDuration;
    }


    // Update is called once per frame
    void Update()
    {
        //if (currentBuffDuration > 0) { currentBuffDuration -= Time.deltaTime; }
        //else { currentBuffDuration = 0; isFired = false; }
        //if (isFired) { currentHealth -= 5; }


    }
}
