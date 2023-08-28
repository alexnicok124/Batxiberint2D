using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Animator animator; 
    public int AttackDamage = 30; //la vida es de 100.
    public float AttackRange = 0.5f; 
    public LayerMask enemyLayers; 
    public Rigidbody2D rbody; 

    public float cooldown = 0.5f; //cooldown
    private float nextAttackTime = 0f; //esto es un default, va variando. 
    void Update()
    {
        if(Time.time >= nextAttackTime){//this if and more things implementa el cooldown
            if(Input.GetKeyDown(KeyCode.Space)){
                attack(); 
                nextAttackTime = Time.time + cooldown; 
            }
        }
    }

    void attack(){
        animator.SetTrigger("TriggerAttack"); 
        Collider2D[] hitenemies = Physics2D.OverlapCircleAll(rbody.position, AttackRange, enemyLayers); 
        
        foreach(Collider2D enemy in hitenemies){
            Debug.Log("we hit" + enemy.name);
            enemy.GetComponent<Enemy>().TakeDamage(AttackDamage);
        }

    }

    void OnDrawGizmosSelected(){
        Gizmos.DrawWireSphere(rbody.position, AttackRange);
    }
}
