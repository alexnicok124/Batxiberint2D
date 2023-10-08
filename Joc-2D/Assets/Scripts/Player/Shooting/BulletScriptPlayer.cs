using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScriptPlayer : MonoBehaviour
{   //aquest script s'activa sempre que es crea una bala: 
    public float speed = 10.0f; 
    public int damage = 20;
    public Rigidbody2D rbody; 
    

    //assignem la velocitat de la bala: 
    void Start()
    {
        rbody.velocity = transform.right * speed; 
    }

    //detecció de col·lisions: 
    void OnTriggerEnter2D(Collider2D HitInfo){
        //si colisiona apliquem un dany a l'enemic: 
        if (HitInfo.GetComponent<HealthEnemy>() != null)
        {
            HitInfo.GetComponent<HealthEnemy>().TakeDamage(damage); 
        }
        //i destruim la bala:
        Destroy(gameObject);
    }

    
}
