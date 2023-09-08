using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;
    int currentHealth;
    public HealthBar healthBar; 
    public GameManagerScript gameManagerScript;
    bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage (int Damage)
    {
        currentHealth -= Damage;
        Debug.Log(currentHealth);
        // Posar animaci� de treure vida
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
    }

    void Die ()
    {
        Debug.Log("Has Mort");

        // Die animation

        //GetComponent<Collider2D>().enabled = false;
        gameManagerScript.GameOver();
        this.enabled = false;
    }
}
