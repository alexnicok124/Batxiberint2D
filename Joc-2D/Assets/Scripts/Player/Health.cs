using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;
    int currentHealth;
    public HealthBar healthBar; 

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

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die ()
    {
        Debug.Log("Has Mort");

        // Die animation

        ////GetComponent<Collider2D>().enabled = false;
        //GetComponent<MovementPlayerScript>().enabled = false;
        //GetComponent<Attack>().enabled = false;
        //GetComponent<Pointing>().enabled = false;
        //GetComponent<Stamina>().enabled = false;
        //this.enabled = false;
    }
}
