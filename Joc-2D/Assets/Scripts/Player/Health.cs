using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    //variables de salud: 
    public int maxHealth;
    int currentHealth;

    //barra de vida: 
    public HealthBar healthBar; 

    //desactivar el personatge: mort
    public GameManagerScript gameManagerScript;
    bool isDead = false; 

    //start es crida al principi. 
    //Configurem la salud i la barra: 
    private void Start()
    {
        
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    //rebre dany dels enemics
    public void TakeDamage (int Damage)
    {
        currentHealth -= Damage;
        Debug.Log(currentHealth);
        
        healthBar.SetHealth(currentHealth);

        //condició de mort: 
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
    }

    //mètode per morir: 
    void Die ()
    {
        Debug.Log("Has Mort");

        gameManagerScript.GameOver();
        this.enabled = false;
    }
}
