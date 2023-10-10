using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    //variables de dashing:
    public bool PlayerIsDashing = false; 
    public float DashSpeed = 10.0f; 
    public float DashDuration = 2.0f; 
    public float DashCooldown = 4.0f; 
    public float ConsumedStamina = 30.0f; 
    private float NextDashTime = 3.0f; 
    private bool Inmune = false; 
    private Vector2 Velocity; 
    
    //layers dels enemics: 
    public LayerMask EnemyLayers;
    private LayerMask EmptyLayer;

    //altres scripts: 
    public Pointing PointingScript; 
    public float DirectionDegrees; 
    public Stamina StaminaScript; 
    public Rigidbody2D rbody; 
    

    public Animator animator; 

    void Update()
    {
        //dashing:
        ManageDash(); 

        //animacions: 
        animator.SetBool("IsDashing", PlayerIsDashing); 

        //esquivem els enemics: 
        if(PlayerIsDashing)
            GetComponent<Collider2D>().excludeLayers = EnemyLayers;
        else if (!PlayerIsDashing)
            GetComponent<Collider2D>().excludeLayers = EmptyLayer;
    }
    

    bool OneTime = true; 
    float Direction, EndTime; 
    void ManageDash(){
        //si decidim fer dashing: 
        if(Input.GetMouseButton(1) && Input.GetKey(KeyCode.E) && (Time.time >= NextDashTime) && (StaminaScript.StaminaPoints >= 30.0f)){
            //bloc que s'executa un cop cada dash: 
            if(OneTime){ 
                //s'encarrega d'assignar unes variables un cop, per evitar errors:
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
            //quan es termina el dash: 
            else{
                //apliquem un període d'espera:
                NextDashTime = Time.time + DashCooldown;
                //aturem el dash: 
                PlayerIsDashing = false; 
                //permetem iniciar-lo de nou: 
                OneTime = true;
            }

        }
        //continuem fent dash:
        else if(Time.time <= EndTime && PlayerIsDashing){
            PlayerIsDashing = true; 
        }
        else{ //aturem el dash. 
            PlayerIsDashing = false; 
        }
       
    }

    //fixed update: el cridem després d'update: 
    void FixedUpdate(){
        //si està dasheant: 
        if(PlayerIsDashing){ 
            //establim una velocitat:  
            rbody.velocity = Velocity * DashSpeed;        
        }
        //si no: 
        else{
            //aturem el moviment: 
            rbody.velocity = new Vector2(0f, 0f); 
        }
    }
}
