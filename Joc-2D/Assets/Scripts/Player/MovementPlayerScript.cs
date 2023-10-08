using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPlayerScript : MonoBehaviour
{
    private float movespeed = 4.0f; //velocitat de moviment
    public float default_speed = 4.0f; //velocitat per defecte
    public Rigidbody2D rbody; //cos físic 2d del personatge
    Vector2 movement; //entrada de dades
    public Animator animator; //animacions

    private Vector2 direction = new Vector2(0, 0); 
    //direction: variable que permet algunes animacions més complicades

    public bool PlayerCanSprint; //està sprinteant?
    public Dashing DashingScript; //referència a classe
    

    //mètode update, es crida cada frame: 
    void Update() => ManageMovementInput(); 


    void ManageMovementInput(){
        if(Input.GetMouseButton(1)){ //si apunta:
            ManagePointingMovement(); 
        }
        else if(Input.GetKey(KeyCode.LeftShift) && PlayerCanSprint){ //si fa sprint:
            ManageSprintMovement();
        }
        else{ //per defecte: caminant i quiet
            ManageWalkingIdle(); 
        }
    }


    void ManageWalkingIdle(){ //caminar i quiet
        //assignació de la velocitat
        movespeed = default_speed; 
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical"); 
        movement = movement.normalized; 
        
        
        //gestió de les animacions: 

        if(movement == Vector2.zero){ //si no hi ha moviment: posició quieta: 
            animator.SetTrigger("Idle"); 
            animator.SetFloat("HorizontalDirection", direction.x);
            animator.SetFloat("VerticalDirection", direction.y); 
        }
        else{ //si no: caminant
            direction = movement; 
            animator.SetTrigger("Walking"); 
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            if(Input.GetKey(KeyCode.F) && PlayerCanSprint){
                animator.SetTrigger("Sprinting"); 
            }
        }
    }

    //en el cas de que apunti: 
    void ManagePointingMovement(){
        //assignació de la velocitat 
         movespeed = default_speed/2.0f; 
         movement.x = Input.GetAxisRaw("Horizontal");
         movement.y = Input.GetAxisRaw("Vertical"); 
         movement = movement.normalized;

        //dades útils per les animacions
         if(movement != Vector2.zero){
            direction = movement; 
         }
         
    }

    void ManageSprintMovement(){ 
        //assignació de la velocitat: 
        movespeed = default_speed * 3.0f; 
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical"); 
        movement = movement.normalized; 

        //gestió de les animacions: 
        if(movement != Vector2.zero){ //si hi ha moviment: 
            animator.SetTrigger("Sprinting");
            direction = movement; 
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }
        else{//si no: el personatge està quiet: 
            ManageWalkingIdle();  
        }
        
    }

    //un cop es crida Update, i s'assigna la velocitat i el moviment, 
    //cridem FixedUpdate per moure el personatge
    void FixedUpdate(){ 
        if(!DashingScript.PlayerIsDashing){  //si està dasheant es desactiva el mètode. 
            //es fa moure al personatge en funció de la velocitat movespeed
            rbody.MovePosition(rbody.position + movement * movespeed * Time.fixedDeltaTime); 
        } 
    }   
}
