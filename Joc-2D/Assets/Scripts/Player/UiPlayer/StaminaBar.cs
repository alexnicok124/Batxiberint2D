using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class StaminaBar : MonoBehaviour
{
    public Slider slider; 
     
    
    public void SetMaxStamina(float MaxStamina){
        slider.maxValue = MaxStamina;
        
    }

    public void SetStamina(float Stamina){
        slider.value = Stamina; 
    
    }
}

