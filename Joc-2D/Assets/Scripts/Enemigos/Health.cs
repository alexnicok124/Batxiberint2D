using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEnemy : MonoBehaviour
{
    public int maxHealth;
    int currentHealth;
    public bool viu = true;
    public LayerMask deadEnemyLayer;
    Animator animator;
    new Rigidbody2D rigidbody;
    new Transform transform;
    EnemySpawner EnemySpawner;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        transform = GetComponent<Transform>();
        rigidbody = GetComponent<Rigidbody2D>();
        EnemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
    }

    public void TakeDamage (int Damage)
    {
        currentHealth -= Damage;
        // Posar animació de treure vida
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die ()
    {
        rigidbody.velocity = Vector3.zero;
        viu = false;
        EnemySpawner.numberOfEnemys--;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.01f);
        gameObject.layer = 8;
        this.enabled = false;
    }
}
