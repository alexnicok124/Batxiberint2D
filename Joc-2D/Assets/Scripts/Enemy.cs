using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxLifePoints = 100; 
    private int currentHealth;

    void Start(){
        currentHealth = maxLifePoints; 
    }
    public void TakeDamage(int damage){ //un setter para modificar la vida del objeto desde otro script
        currentHealth -= damage; 
        if(currentHealth <= 0){
            die(); 
        }
    }

    void die(){
        Debug.Log("the enemy has died"); 
    }
}
