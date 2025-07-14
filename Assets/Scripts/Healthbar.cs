using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class Healthbar : MonoBehaviour {

    public Slider Slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxHealth(int health,int maxhealth)
    {
        Slider.maxValue = maxhealth;
        Slider.minValue = health;
        fill.color = gradient.Evaluate(1f);
    }
    public void SetHealth(int health)
    {
        Slider.value = health;
        fill.color = gradient.Evaluate(Slider.normalizedValue);  
    }
}