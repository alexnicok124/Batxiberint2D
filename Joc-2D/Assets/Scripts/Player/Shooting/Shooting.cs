using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform FirePoint;
    public GameObject BulletPrefab; 
    public UpdateBullets bulletsscript; 
    public bool CanShoot = true; 

    public float cooldown = 0.3f; 
    private float nextAttackTime = 0f;
    void Update(){

        if(Time.time >= nextAttackTime){
            if(Input.GetMouseButton(0) && bulletsscript.Bullets > 0 && CanShoot){
                Shoot(); 
                nextAttackTime = Time.time + cooldown; 
            }
        }
        
    }


    void Shoot(){
        FirePoint.GetComponent<Animator>().SetTrigger("Shoot");
        Instantiate(BulletPrefab, FirePoint.position, transform.rotation); 
        bulletsscript.Bullets--;
        bulletsscript.hasShoot = true;
    }
    
}
