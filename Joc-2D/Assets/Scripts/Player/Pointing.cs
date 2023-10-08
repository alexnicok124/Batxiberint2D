using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointing : MonoBehaviour //classe que gestiona les animacions i la rotació quan apunta: 
{
    //variables internes: 
    private Vector3 MouseTarget;
    public new Camera camera;
    
    public float RotationMouse = 0; 
    public float Degrees; 

    public Dashing DashingScript; 
    public Animator animator; 

    //update: es crida cada frame: 
    void Update()
    {
        //activem la rotació (ratolí esquerra)
        if(Input.GetMouseButton(1)){ 
            ManageMouseRotation(); 
            
        }
        //si no apunta: 
        else{
            //eliminem la rotació: 
            transform.rotation = Quaternion.Euler(0, 0, 0); 
        }
        
    }

    void ManageMouseRotation(){
        //animacions: 
        if(!DashingScript.PlayerIsDashing){
          animator.SetTrigger("TriggerPointing");   
        }
        
        //calculem l'angle que ha de rotar segons la posició del ratolí: 
        MouseTarget = camera.ScreenToWorldPoint(Input.mousePosition); 
        RotationMouse = Mathf.Atan2((MouseTarget.y - transform.position.y), 
        (MouseTarget.x - transform.position.x));

        Degrees = RotationMouse * (180 / Mathf.PI ); 
        transform.rotation = Quaternion.Euler(0, 0, Degrees);
    }
}
