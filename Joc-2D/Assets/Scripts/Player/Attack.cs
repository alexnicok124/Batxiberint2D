using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//para atacar a meleé. 

public class Attack : MonoBehaviour
{
    public Animator animator; 
    public int AttackDamage = 30; //la vida es de 100.
    public float AttackRange = 0.5f; 
    public LayerMask enemyLayers; 
    public Rigidbody2D rbody; 
    public Transform AttackPoint; 

    public float cooldown = 0.5f; //cooldown
    private float nextAttackTime = 0f; //esto es un default, va variando. 
    void Update()
    {
        
        if(Time.time >= nextAttackTime){//this if and more things implementa el cooldown
            if(Input.GetMouseButton(0)){//botó esquerra del ratolí per a atacar. 
                attack(); 
                nextAttackTime = Time.time + cooldown; 
            }
        }
    }

#pragma warning disable IDE1006 // Estilos de nombres
    void attack(){
        AttackPoint.GetComponent<Animator>().SetTrigger("Attack");
        Debug.Log("Attacked");
        Collider2D[] hitenemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, enemyLayers); 
        
        foreach(Collider2D enemy in hitenemies){
            Debug.Log("we hit" + enemy.name);
            if (enemy.GetComponent<HealthEnemy>() != null)
            {
                enemy.GetComponent<HealthEnemy>().TakeDamage(AttackDamage);
            }
        }

    }
#pragma warning restore IDE1006 // Estilos de nombres

    void OnDrawGizmosSelected(){
        if(AttackPoint == null)
            return; 
        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }
}
