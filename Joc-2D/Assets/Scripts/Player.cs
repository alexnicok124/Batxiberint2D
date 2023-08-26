using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 3.5f;
    public int maxHealth = 100;
    int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    //Update Funtion
    void Update()
    {
        //movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0f);

        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void TakeDamage(int Damage)
    {
        currentHealth -= Damage;
        Debug.Log(currentHealth);

        // Posar aqui l'animació de rebre mal

        // Comprobar si ha mort
        if (currentHealth <= 0)
        {
            die();
        }
    }

    void die()
    {
        Debug.Log("Has mort!");

        // Animació de morir:

        // Desactivar l'enemic
    }
}
