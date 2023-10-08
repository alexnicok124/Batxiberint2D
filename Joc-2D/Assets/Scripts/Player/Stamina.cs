using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    //variables d'estamina: 
    public float MaxStaminaPoints = 100.0f; 
    public float StaminaPoints; 
    public float RegenerationSpeed = 3.0f; 
    public float ConsumingSpeed = 5.0f; 

    //altres variables: 
    private Vector2 movement; 
    public MovementPlayerScript MovScript; 
    public StaminaBar Bar; 

    //mètode Start, configurem l'estamina: 
    void Start(){
        StaminaPoints = MaxStaminaPoints;
        Bar.SetMaxStamina(MaxStaminaPoints);
    }


    //mètode update: es crida cada frame: 
    void Update()
    {
        //gestionem la barra d'estamina: 
        Bar.SetStamina(StaminaPoints);

        //si fem sprint: 
        if(Input.GetKey(KeyCode.LeftShift) && StaminaPoints > 30.0f){ 
            //reduim punts d'estamina:
            ReduceStaminaPoints(); 
            
            //configurem el moviment com "sprint": 
            MovScript.PlayerCanSprint = true; 
        }
        //si no tenim suficient estamina: 
        else if(StaminaPoints <= 30.0f){
            //no podem moure'ns
            MovScript.PlayerCanSprint = false; 
            RegenerateStaminaPoints(); 
        }
        //si no fem sprint: 
        else
        {
            //regenerem l'estamina: 
            RegenerateStaminaPoints();             
        }
    }




    //mètodes: 

    void ReduceStaminaPoints(){
        //si no hi ha moviment: regenerem l'estamina: 
        movement.x = Input.GetAxisRaw("Horizontal"); 
        movement.y = Input.GetAxisRaw("Vertical");
        if(movement == Vector2.zero){
            RegenerateStaminaPoints();  
        }
        //si hi ha moviment: la consumim: 
        else{
            StaminaPoints -= ConsumingSpeed * Time.deltaTime; 
        }
    }


    
    //regeneració d'estamina:
    void RegenerateStaminaPoints(){
        if(StaminaPoints < MaxStaminaPoints){ 
            StaminaPoints += RegenerationSpeed * Time.deltaTime; 
        }
        else{
            StaminaPoints = MaxStaminaPoints; 
        }
    }

    //mètode per modificar l'estamina des d'altres classes:
    public void ChangeStamina(float Change){
        StaminaPoints += Change; 
    }
}