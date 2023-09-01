using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEnemy : MonoBehaviour
{
    public int maxHealth;
    int currentHealth;
    public bool viu = true;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage (int Damage)
    {
        currentHealth -= Damage;
        Debug.Log(currentHealth);
        // Posar animació de treure vida

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die ()
    {
        Debug.Log("Enemic Mort");

        viu = false;
        this.enabled = false;
    }
}
