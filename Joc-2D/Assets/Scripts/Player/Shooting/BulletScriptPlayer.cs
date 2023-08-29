using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScriptPlayer : MonoBehaviour
{
    public float speed = 10.0f; 
    public int damage;
    public Rigidbody2D rbody; 
    


    void Start()//cada cop que se instancia
    {
        rbody.velocity = transform.right * speed; 
        //la dreta es on apunta el personatge sempre
    }

    void OnTriggerEnter2D(Collider2D HitInfo){
        Debug.Log(HitInfo.name); 
        Destroy(gameObject); 

        
    }

    
}
