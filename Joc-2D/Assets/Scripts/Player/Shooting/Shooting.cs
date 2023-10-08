using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{   //aquest script es per atacar amb la pistola
    //cal tenir la pistola activada per disparar

    
    //variables internes:
    public Transform FirePoint;
    public GameObject BulletPrefab; 
    public UpdateBullets bulletsscript; 
    public bool CanShoot = true; 

    //variables de temps d'espera: 
    public float cooldown = 0.3f; 
    private float nextAttackTime = 0f;

    //mètode update: es crida cada frame: 
    void Update(){
        //disparem (amb el ratolí dret) i apliquem un temps d'espera: 
        if(Time.time >= nextAttackTime){
            if(Input.GetMouseButton(0) && bulletsscript.Bullets > 0 && CanShoot){
                Shoot(); 
                nextAttackTime = Time.time + cooldown; 
            }
        }
        
    }

    //mètode per disparar:
    void Shoot(){
        //animacions: 
        FirePoint.GetComponent<Animator>().SetTrigger("Shoot");

        //creació de la bala: 
        Instantiate(BulletPrefab, FirePoint.position, transform.rotation); 

        //gestió de la quantitat de bales: 
        bulletsscript.Bullets--;
        bulletsscript.hasShoot = true;
    }
    
}
