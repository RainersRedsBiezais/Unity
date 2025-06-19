using UnityEngine;
using UnityEngine.Events;

public class BlacksmithHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public UnityEvent onDeath;
    public GameObject damageNumberPrefab;
    public Canvas uiCanvas; // Assign your main UI canvas here
    public GameObject gameOverPanel; // Assign in Inspector

    void Awake()
    {   
        currentHealth = maxHealth;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        ShowDamageNumber(amount);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            if (onDeath != null)
                onDeath.Invoke();
            if (gameOverPanel != null)
                gameOverPanel.SetActive(true);
        }
    }

    void ShowDamageNumber(int amount)
    {
        if (damageNumberPrefab != null && uiCanvas != null)
        {
            // Convert world position to screen position
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2f);
            // Instantiate as a child of the UI Canvas
            GameObject dmgObj = Instantiate(damageNumberPrefab, uiCanvas.transform);
            // Set the position of the RectTransform to the screen position
            RectTransform rect = dmgObj.GetComponent<RectTransform>();
            rect.position = screenPos;
            // Set the text
            var dmgScript = dmgObj.GetComponent<DamageNumber>();
            if (dmgScript != null)
                dmgScript.SetText(amount.ToString());
            else
                Debug.LogError("DamageNumber script missing on prefab!");
        }
        else
        {
            Debug.LogWarning("DamageNumberPrefab or UiCanvas is not assigned!");
        }
    }
}