using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPlayerScript : MonoBehaviour
{
    private float movespeed = 4.0f; 
    public float default_speed = 4.0f; 
    public Rigidbody2D rbody;
    Vector2 movement;
    public Animator animator;
    private Vector2 direction = new Vector2(0, 0); 
    public bool PlayerCanSprint; //variable que permite sprintear
    
    void Update() => ManageMovementInput();


    void ManageMovementInput(){
        if(Input.GetMouseButton(1)){
            ManagePointingMovement(); 
        }
        else if(Input.GetMouseButton(0) && PlayerCanSprint){ //con el ratón funciona, pero en realidad el clic izquierdo es para atacar
            ManageSprintMovement(); 
        }
        else{
            ManageWalkingIdle(); 
        }
    }


    void ManageWalkingIdle(){ //walking and idle
        movespeed = default_speed; 
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical"); 
        movement = movement.normalized; 
        
        
        if(movement == Vector2.zero){ //manage idle
            animator.SetTrigger("Idle"); 
            animator.SetFloat("HorizontalDirection", direction.x);
            animator.SetFloat("VerticalDirection", direction.y); 
        }
        else{ //manage walking
            direction = movement; 
            animator.SetTrigger("Walking"); 
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }
    }

    void ManagePointingMovement(){
         movespeed = default_speed/2.0f; 
         movement.x = Input.GetAxisRaw("Horizontal");
         movement.y = Input.GetAxisRaw("Vertical"); 
         movement = movement.normalized; 
         
    }

    void ManageSprintMovement(){ 
        animator.SetTrigger("Sprinting"); 
        movespeed = default_speed * 3.0f; 
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical"); 
        movement = movement.normalized; 
        
    }

    void FixedUpdate(){ 
        rbody.MovePosition(rbody.position + movement * movespeed * Time.fixedDeltaTime); 
    }   
}
