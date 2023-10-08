using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Attack : MonoBehaviour 
{   //aquest script es per atacar amb la espassa
    //cal tenir la espassa activada per atacar

    public Animator animator; //animacions

    //variables de l'espassa: 
    public int AttackDamage = 30; 
    public float AttackRange = 0.5f; 
    public LayerMask enemyLayers; 
    public Rigidbody2D rbody;
    public Transform sword;
    public Transform attackPoint; 

    //temps d'espera: 
    public float cooldown = 0.5f; 
    private float nextAttackTime = 0f;

    //mètode update, es crida cada frame: 
    void Update()
    {
        //ataquem (amb el ratolí dret) i apliquem un temps d'espera
        if(Time.time >= nextAttackTime){
            if(Input.GetMouseButton(0)){
                attack(); 
                nextAttackTime = Time.time + cooldown; 
            }
        }
    }

#pragma warning disable IDE1006 // Estils de noms
    
    //mètode per atacar:
    void attack(){
        //animacions espassa:
        sword.GetComponent<Animator>().SetTrigger("Attack");

        //detecció d'enemics: 
        Collider2D[] hitenemies = Physics2D.OverlapCircleAll(attackPoint.position, AttackRange, enemyLayers); 
        
        //apliquem dany a cada enemic detectat:
        foreach(Collider2D enemy in hitenemies) 
        {
            if (enemy.GetComponent<HealthEnemy>() != null)
            {
                enemy.GetComponent<HealthEnemy>().TakeDamage(AttackDamage);
            }
            if (enemy.CompareTag("Projectile"))
            {
                Debug.Log("Cut the projectile");
                enemy.GetComponent<Projectile>().DestroyProjectile();
            }
        }

    }
#pragma warning restore IDE1006 // Estils de noms

    //mètode per dibuixar en el editor, no afecta al joc:
    void OnDrawGizmosSelected(){
        if(attackPoint == null)
            return; 
        Gizmos.DrawWireSphere(attackPoint.position, AttackRange);
    }
}
