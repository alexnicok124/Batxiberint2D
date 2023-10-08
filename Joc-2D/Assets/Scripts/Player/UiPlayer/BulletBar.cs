using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class BulletBar : MonoBehaviour
{
    //slider
    public Slider slider; 
    
    //mètodes
    public void SetMaxTime(float LoadingTime){
        slider.maxValue = LoadingTime;
    }

    public void SetTime(float CurrentLoading){
        slider.value = CurrentLoading; 
    }
}
