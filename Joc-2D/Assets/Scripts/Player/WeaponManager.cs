using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour 
{
    //imatges de la espasa i pistola, per activar o desactivar
    public GameObject SwordObject, GunObject; 

    //scripts per desactivar
    public Attack MeleeAttack;
    public Shooting ShootingScript;

    //variables booleanes:
    private bool ActSword = false, ActGun = false; 

    //Activem la espassa per defecte
    void Start(){
        ActSword = true; 
        MeleeAttack = GetComponent<Attack>(); 
        ShootingScript = GetComponent<Shooting>();
    }

    void Update()
    {
        //seleccionar amb 1 = espasa o 2 = pistola l'arma de joc
        if(Input.GetKeyDown(KeyCode.Alpha1)){  
            ActSword = true; 
            ActGun = false;
            Debug.Log("Espasa seleccionada");
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2)){
            ActGun = true;
            ActSword = false; 
            Debug.Log("Pistola seleccionada"); 
        }

        //activem l'arma seleccionada
        if(Input.GetMouseButton(1)){ 
            if(ActSword){
                ActivateSword();
            }
            else if(ActGun){
                ActivateGun(); 
            }
        }
        //en el cas de que no estigui en posició d'atac:
        else{
            //es desactiva tot: 
            DisableAll(); 
        }
    }



    //mètodes: 
    void ActivateSword(){ 
        SwordObject.SetActive(true); 
        GunObject.SetActive(false); 
        MeleeAttack.enabled = true;
        ShootingScript.enabled = false;  
    }
    void ActivateGun(){
        GunObject.SetActive(true); 
        SwordObject.SetActive(false);
        MeleeAttack.enabled = false;
        ShootingScript.enabled = true; 
    }

    void DisableAll(){
        GunObject.SetActive(false); 
        SwordObject.SetActive(false);
        MeleeAttack.enabled = false;
        ShootingScript.enabled = false;
    }
    
}
