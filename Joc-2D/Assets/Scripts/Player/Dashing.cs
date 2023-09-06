using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    public bool PlayerIsDashing = false; 
    public float DashSpeed = 10.0f; 
    public float DashDuration = 2.0f; 
    public float DashCooldown = 4.0f; 
    public float ConsumedStamina = 30.0f; 
    public bool Inmune = false;
    public LayerMask EnemyLayers;
    private LayerMask EmptyLayer;
    private Vector2 Velocity; 


    public Pointing PointingScript; 
    public float DirectionDegrees; 
    public Stamina StaminaScript; 
    public Rigidbody2D rbody; 
    private float NextDashTime = 3.0f; 

    public Animator animator; 

    void Update()
    {
        ManageDash(); 
        animator.SetBool("IsDashing", PlayerIsDashing); 
        if(PlayerIsDashing)
            GetComponent<Collider2D>().excludeLayers = EnemyLayers;
        else if (!PlayerIsDashing)
            GetComponent<Collider2D>().excludeLayers = EmptyLayer;
    }
    

    bool OneTime = true; 
    float Direction, EndTime; 
    void ManageDash(){
        if(Input.GetMouseButton(1) && Input.GetKey(KeyCode.E) && (Time.time >= NextDashTime) && (StaminaScript.StaminaPoints >= 30.0f)){
    
            if(OneTime){ 
                Direction = PointingScript.RotationMouse;
                StaminaScript.ChangeStamina(-ConsumedStamina);
                Velocity = new Vector2(Mathf.Cos(Direction), Mathf.Sin(Direction)).normalized;
                
                EndTime = Time.time + DashDuration;
            }
            OneTime = false; 

            //mentres estigui en DashDuration, permetem al player dashear 
            if(Time.time <= EndTime){
                PlayerIsDashing = true;
                
            }
            else{
                 
                NextDashTime = Time.time + DashCooldown;
                PlayerIsDashing = false; 
                OneTime = true;
            }

        }
        else if(Time.time <= EndTime && PlayerIsDashing){
            PlayerIsDashing = true; //para que si dejamos de presionar la tecla siga haciendo el dash si estaba dasheando
        }
        else{
            PlayerIsDashing = false; 
        }
       
    }

    
    void FixedUpdate(){
        if(PlayerIsDashing){            
            rbody.velocity = Velocity * DashSpeed;        
        }
        else{
            rbody.velocity = new Vector2(0f, 0f); 
        }
    }
}
