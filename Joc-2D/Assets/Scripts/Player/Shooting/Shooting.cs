using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform FirePoint;  //en la punta del objecte pistola, asignar el hijo de player = gun 
    public GameObject BulletPrefab; 




    public float cooldown = 0.3f; //cooldown
    private float nextAttackTime = 0f;
    void Update(){

        if(Time.time >= nextAttackTime){//this if and more things implementa el cooldown
            if(Input.GetMouseButton(0)){//botó esquerra del ratolí per a atacar. 
                Shoot(); 
                nextAttackTime = Time.time + cooldown; 
            }
        }
        
    }


    void Shoot(){
        Instantiate(BulletPrefab, FirePoint.position, FirePoint.rotation); //mirar si la rotación está bien, como sea. 
    }
}
