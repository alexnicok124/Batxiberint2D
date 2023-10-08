using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class StaminaBar : MonoBehaviour
{
    //slider
    public Slider slider; 
     
    //mètodes
    public void SetMaxStamina(float MaxStamina){
        slider.maxValue = MaxStamina;
        
    }

    public void SetStamina(float Stamina){
        slider.value = Stamina; 
    
    }
}

