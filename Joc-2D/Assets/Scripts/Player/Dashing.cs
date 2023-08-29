using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    public bool PlayerIsDashing = false; 
    public float DashSpeed = 10.0f; 
    public float DashDuration = 2.0f; 
    public float DashCooldown = 0.1f; 
    public float ConsumedStamina = 30.0f; 
    public bool Inmune = false; 
    private Vector2 Velocity; 


    public Pointing PointingScript; //para hacer: asignar referencia
    public float DirectionDegrees; 
    public Stamina StaminaScript; 
    public Rigidbody2D rbody; 
    private float NextDashTime = 3.0f; 




    void Start()
    {
        return; 
    }

    // Update is called once per frame
    void Update()
    {
        ManageDash(); 
    }
    

    bool OneTime = true; 
    float Direction, EndTime; //variables útiles para este método. 
    void ManageDash(){
        
        if(Input.GetMouseButton(1) && Input.GetKey(KeyCode.L) && (Time.time >= NextDashTime)){
            //Debug.Log("Checkpoint 4");

            if(OneTime){ //un cop per atac sempre
                Direction = PointingScript.RotationMouse;
                StaminaScript.ChangeStamina(-ConsumedStamina);
                Velocity = new Vector2(Mathf.Cos(Direction), Mathf.Sin(Direction)).normalized;
                //Debug.Log("he llegado hasta aqui 1");  
                EndTime = Time.time + DashDuration;
            }
            OneTime = false; 


            //mentres estigui en DashDuration, permetem al player a dashear 
            if(Time.time <= EndTime){
                PlayerIsDashing = true;
                //Debug.Log("Checkpoint 3"); 
                
            }
            else{
                //ya se ha terminado, paramos
                //Debug.Log("checkpoint 2"); 
                NextDashTime = Time.time + DashCooldown;
                PlayerIsDashing = false; 
                OneTime = true;
            }

        }
        else{
            PlayerIsDashing = false; 
        }
        

    }

    /*
    En teoria todo funciona bien, aunque no sé porque si le asigno la velocidad el jugador no se mueve el pelotudo. 
    tiene pinta de que es por el rbody.MovePosition, si moviment = 0, se mantiene igual ahí... 
    */

    //les físiques
    void FixedUpdate(){
        if(PlayerIsDashing){
            //Debug.Log("velocidad asignada"); 
            rbody.velocity = Velocity * DashSpeed;    
            //Debug.Log(rbody.velocity); 
        }
        else{
            rbody.velocity = new Vector2(0f, 0f); 
        }
    }
}
