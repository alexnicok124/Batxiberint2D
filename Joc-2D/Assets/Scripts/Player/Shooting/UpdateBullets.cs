using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateBullets : MonoBehaviour
{
    public Shooting ShootingScript;
    public GameObject[] ammoUI;

    public float LoadingTime = 1.2f; 
    private float endLoadingTime = 0.0f;
    private bool flagonetime = true;
    public bool hasShoot = false;
    public int Bullets;
    public int MaxBullets = 10;
    public int bulletsPerReload = 1;

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
        if (hasShoot)
        {
            hasShoot = false;
            ammoUI[Bullets].SetActive(false);
        }
    }

    private float Timer = 0.0f; //per a la barra
    void LoadingBullets()
    {
        if(Input.GetKey(KeyCode.R) && Bullets != MaxBullets)
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
                Bullets += bulletsPerReload; 
                if(Bullets > MaxBullets){
                    Bullets = MaxBullets; 
                }
                ammoUI[Bullets-1].SetActive(true);
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
