using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class HealthBar : MonoBehaviour
{
    //slider
    public Slider slider; 

    //colors
    public Gradient gradient; 
    public Image fill; 
    
    //mètodes
    public void SetMaxHealth(int MaxHealth){
        slider.maxValue = MaxHealth;
        slider.value = MaxHealth;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int Health){
        slider.value = Health; 
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
