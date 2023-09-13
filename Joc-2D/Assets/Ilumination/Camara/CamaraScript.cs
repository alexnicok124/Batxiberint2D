using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraScript : MonoBehaviour
{
    public Transform target; 
    public Vector3 offset; 

    void Start(){
        offset = new Vector3(0, 0, -10); 
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset; 
    }
}
