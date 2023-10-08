using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateBullets : MonoBehaviour
{
    //referències: 
    public Shooting ShootingScript;
    public GameObject[] ammoUI;

    //variables de les bales: 
    public float LoadingTime = 1.2f; 
    private float endLoadingTime = 0.0f;
    private bool flagonetime = true;
    bool reloading = false;
    public bool hasShoot = false;
    public int Bullets;
    public int MaxBullets = 10;
    public int bulletsPerReload = 1;

    //animacions i imatges: 
    public BulletBar Bar; 
    public GameObject UIobject;
    public Animator gunAnimator;

    //configurem les quantitats de bales al principi:
    void Start()
    {
        Bullets = MaxBullets; 
        Bar.SetMaxTime(LoadingTime); 
        Bar.SetTime(0.0f); 
    }
    

    void Update()
    {
        //gestionem la recàrrega de bales: 
        LoadingBullets();
        if (hasShoot)
        {
            hasShoot = false;
            ammoUI[Bullets].SetActive(false);
        }
    }

    private float Timer = 0.0f; 
    //càrrega de bales: 
    void LoadingBullets()
    {
        //si recargem (amb la tecla R):
        if(Input.GetKey(KeyCode.R) && Bullets != MaxBullets && !reloading)
        {
            //mostrem la barra de recarga: 
            UIobject.SetActive(true); 

            //desactivem la funció de disparar: 
            ShootingScript.CanShoot = false; 

            //asignem el temps de càrrega una vegada:
            if(flagonetime){
                endLoadingTime = Time.time + LoadingTime; 
                flagonetime = false;
            }
            //visualitzem el progrés de recarrega en la barra
            Timer += Time.deltaTime;
            Bar.SetTime(Timer);
            
            //quan hem terminat de recargar: 
            if(Time.time > endLoadingTime)
            {
                //animacions:
                gunAnimator.SetTrigger("Reload");

                //augmentem la quantitat de bales:
                Bullets += bulletsPerReload; 
                if(Bullets > MaxBullets){
                    Bullets = MaxBullets; 
                }

                //assignem altres variables importants: 
                ammoUI[Bullets-1].SetActive(true);
                Timer = 0.0f;
                reloading = true;

                //assignem el seguent temps de recarga:
                flagonetime = true; 
            }

        }
        //si no està recargant: 
        else{
            //reinicia el comptador
            flagonetime = true; 

            //permetem disparar
            ShootingScript.CanShoot = true; 

            //amagem la barra:
            UIobject.SetActive(false);  

            //altres variables:
            Timer = 0.0f; 
            reloading = false;
        }
    }
}
