using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateBullets : MonoBehaviour
{
    public Shooting ShootingScript; 

    private float LoadingTime = 1.2f; 
    private float endLoadingTime = 0.0f;
    private bool flagonetime = true; 
    public int Bullets;
    public int MaxBullets = 10; 

    public BulletBar Bar; 
    public GameObject UIobject; 


    void Start()
    {
        Bullets = MaxBullets; 
        Bar.SetMaxTime(LoadingTime); 
        Bar.SetTime(0.0f); 
    }
    
    void Update()
    {
        LoadingBullets(); 
    }

    private float Timer = 0.0f; //per a la barra
    void LoadingBullets()
    {
        if(Input.GetKey(KeyCode.R))
        {
            UIobject.SetActive(true); 
            ShootingScript.CanShoot = false; 
            if(flagonetime){
                endLoadingTime = Time.time + LoadingTime; 
                flagonetime = false; 
            }
            Timer += Time.deltaTime; 
            Bar.SetTime(Timer);
            
            if(Time.time > endLoadingTime){
                Bullets += 5; 
                if(Bullets > MaxBullets){
                    Bullets = MaxBullets; 
                }
                Timer = 0.0f; 

                flagonetime = true; //set next time attack
            }

        }
        else{
            flagonetime = true; //reinicia el comptador
            ShootingScript.CanShoot = true; 
            UIobject.SetActive(false);  
            Timer = 0.0f; 
        }
    }
}
