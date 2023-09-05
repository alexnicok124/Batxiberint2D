using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEnemy : MonoBehaviour
{
    public int maxHealth;
    int currentHealth;
    public bool viu = true;
    Animator animator;
    new Rigidbody2D rigidbody;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage (int Damage)
    {
        currentHealth -= Damage;
        Debug.Log(currentHealth);
        // Posar animació de treure vida
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die ()
    {
        Debug.Log("Enemic Mort");
        rigidbody.velocity = Vector3.zero;
        viu = false;
        this.enabled = false;
    }
}
