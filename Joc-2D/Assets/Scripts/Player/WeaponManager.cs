using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour //como un inventario donde manejo todas las armas
{
    //asignar al jugador, queda per fer-ho. 
    public GameObject SwordObject, GunObject;
    public Attack MeleeAttack;
    public Shooting ShootingScript;
    private bool ActSword = false, ActGun = false; 
    void Start(){
        ActSword = true; 
        MeleeAttack = GetComponent<Attack>(); 
        ShootingScript = GetComponent<Shooting>();
    }

    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Alpha1)){ //1, 2, 3, como el inventario del minecraft que te permite seleccionar cada cosa. 
            ActSword = true; 
            ActGun = false;
            Debug.Log("Espasa seleccionada");
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2)){
            ActGun = true;
            ActSword = false; 
            Debug.Log("Pistola seleccionada"); 
        }

        if(Input.GetMouseButton(1)){ // solo se puede atacar si estoy apuntando antes, cuando apunto, me aparecen las armas. 
            if(ActSword){
                ActivateSword();
            }
            else if(ActGun){
                ActivateGun(); 
            }
        }
        else{
            DisableAll(); 
        }
    }

    void ActivateSword(){
        SwordObject.SetActive(true); 
        GunObject.SetActive(false); 
        MeleeAttack.enabled = true;
        ShootingScript.enabled = false;  //Desactivo aquest script per a que no pugui tener el comportament d'atacar.
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
