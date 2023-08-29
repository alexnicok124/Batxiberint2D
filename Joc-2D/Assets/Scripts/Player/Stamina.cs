using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    public float MaxStaminaPoints = 100.0f; 
    public float StaminaPoints; 
    public float RegenerationSpeed = 3.0f; 
    public float ConsumingSpeed = 5.0f; 
    private Vector2 movement; 
    public MovementPlayerScript MovScript; //asignar la referencia

    void Start(){
        StaminaPoints = MaxStaminaPoints;
    }

    void Update()
    {
        Debug.Log(StaminaPoints);

        if(Input.GetKey(KeyCode.F) && StaminaPoints > 30.0f){ //aquí es donde puede correr, tengo que asignar CanRunning del otro script como true
            ReduceStaminaPoints(); //mientras corra no puede regenerar stamina. 
            //enable sprinting
            MovScript.PlayerCanSprint = true; 
        }
        else if(StaminaPoints <= 30.0f){
            MovScript.PlayerCanSprint = false; 
            RegenerateStaminaPoints(); 
        }
        else
        {
            RegenerateStaminaPoints();             
        }
    }

    void ReduceStaminaPoints(){
        movement.x = Input.GetAxisRaw("Horizontal"); //sería mejor llamarlo desde otro script la verdad, perobueno, solución chapucerapara acabarlo lo más rapdio
        movement.y = Input.GetAxisRaw("Vertical"); 
        if(movement == Vector2.zero){
            RegenerateStaminaPoints(); 
        }
        else{
            StaminaPoints -= ConsumingSpeed * Time.deltaTime; 
        }
    }

    void RegenerateStaminaPoints(){
        if(StaminaPoints < MaxStaminaPoints){ //no igual no funciona, menor que supongo que si, es float
            StaminaPoints += RegenerationSpeed * Time.deltaTime; 
        }
        else{
            StaminaPoints = MaxStaminaPoints; 
        }
    }

    void ShowStaminaBar(){
        int a = 1 + 1; 
    }

    public void ChangeStamina(float Change){
        StaminaPoints += Change; 
    }
}
