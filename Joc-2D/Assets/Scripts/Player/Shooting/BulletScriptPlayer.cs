using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScriptPlayer : MonoBehaviour
{
    public float speed = 10.0f; 
    public int damage = 20;
    public Rigidbody2D rbody; 
    


    void Start()//cada cop que se instancia
    {
        rbody.velocity = transform.right * speed; 
        //la dreta es on apunta el personatge sempre
    }

    void OnTriggerEnter2D(Collider2D HitInfo){
        if (HitInfo.GetComponent<HealthEnemy>() != null)
        {
            HitInfo.GetComponent<HealthEnemy>().TakeDamage(damage); 
        }
        Destroy(gameObject);
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
    
}
