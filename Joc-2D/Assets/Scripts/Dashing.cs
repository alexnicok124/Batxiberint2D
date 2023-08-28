using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    public bool PlayerIsDashing = false; //cuando dasheemos, tenemos que asignar desde el script de movimiento que estamos haiendolo
    //para evitar problemas, para que no sea pueda mover, 
    //se debe de apuntar antes de dashear, l que haremos será obtener la dirección a la que dashea el personaje
    //y luego cuando dasheemos hará una animación mientras tanto de dashear

    //luego una variable de tiempo que cuando haya terminado el tiempo de dashear, PlayerIsDashing = false; 
    //mientras tanto, = true. 

    // Start is called before the first frame update


    //reduce la stamina
    public float DashSpeed = 10.0f; 
    public float DashDuration = 1.0f; 
    public float DashCooldown = 3.0f; 
    public float ConsumedStamina = 30.0f; 
    public bool Inmune = false; 


    public Pointing PointingScript; //para hacer: asignar referencia
    public float DirectionDegrees; 
    public Stamina StaminaScript; 
    public Rigidbody2D rbody; 




    void Start()
    {
        return; 
    }

    // Update is called once per frame
    void Update()
    {
        ManageDash(); 
    }
    

    
    void ManageDash(){
        if(Input.GetMouseButton(1) && Input.GetKeyDown(KeyCode.L)){
            float Direction = PointingScript.RotationMouse;
            StaminaScript.ChangeStamina(-ConsumedStamina);
            PlayerIsDashing = true;  
            float EndTime = Time.time + DashDuration; 
            Vector2 velocity = new Vector2(Mathf.Cos(Direction), Mathf.Sin(Direction));
            while(Time.time <= EndTime){
                rbody.velocity = velocity * DashSpeed;                
            }
            rbody.velocity = new Vector2(0.0f, 0.0f); 

        }
        else{
            PlayerIsDashing = false; 
        }
        

    }


































    //for physics
    void FixedUpdate(){
        return; 
    }
}
