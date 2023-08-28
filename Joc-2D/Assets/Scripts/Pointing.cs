using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointing : MonoBehaviour
{
    private Vector3 MouseTarget; 
    public Camera camera; 
    //este escript es para el jugador. 
    private float RotationMouse = 0; //angulo que tiene que rotar
    private float Degrees; 
    //puedo hacer que en start, la rotacion por defecto sea 0
    
    public Animator animator; 
    void Update()
    {
        if(Input.GetMouseButton(1)){ // si pulsa el botón derecho del ratón, se pone en modo apuntar. 
            ManageMouseRotation(); 
            
        }
        else{
            //animator.SetTrigger("StopPointingTrigger"); //no sé si está optimizado, canviar por un booleano ? per ara si camines pots sortir.
            transform.rotation = Quaternion.Euler(0, 0, 0); 
        }
        
    }

    void ManageMouseRotation(){
        animator.SetTrigger("TriggerPointing"); 
        MouseTarget = camera.ScreenToWorldPoint(Input.mousePosition); 
        RotationMouse = Mathf.Atan2((MouseTarget.y - transform.position.y), 
        (MouseTarget.x - transform.position.x));

        Degrees = RotationMouse * (180 / Mathf.PI ); 
        transform.rotation = Quaternion.Euler(0, 0, Degrees);
    }



    
    

}
