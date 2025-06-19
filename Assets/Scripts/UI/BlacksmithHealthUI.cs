using UnityEngine;
using UnityEngine.UI;

public class BlacksmithHealthUI : MonoBehaviour
{
    public BlacksmithHealth blacksmithHealth;
    public Slider healthSlider;

    void Start()
    {
        healthSlider.maxValue = blacksmithHealth.maxHealth;
        healthSlider.value = blacksmithHealth.currentHealth;
    }

    void Update()
    {
        healthSlider.value = blacksmithHealth.currentHealth;
    }
}