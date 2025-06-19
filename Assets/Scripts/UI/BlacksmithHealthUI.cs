using UnityEngine;
using UnityEngine.UI;

public class BlacksmithHealthUI : MonoBehaviour
{
    public Slider healthSlider;
    private BlacksmithHealth blacksmithHealth;

    void Start()
    {
        blacksmithHealth = FindObjectOfType<BlacksmithHealth>();
        if (blacksmithHealth == null)
        {
            Debug.LogError("BlacksmithHealth not found!");
            return;
        }

        if (healthSlider == null)
        {
            Debug.LogError("Health slider not assigned!");
            return;
        }

        // Set max health
        healthSlider.maxValue = blacksmithHealth.maxHealth;
        UpdateHealthUI();
    }

    void Update()
    {
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        if (blacksmithHealth != null && healthSlider != null)
        {
            healthSlider.value = blacksmithHealth.GetCurrentHealth();
        }
    }
}