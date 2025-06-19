using UnityEngine;
using UnityEngine.Events;

public class BlacksmithHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;
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

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        ShowDamageNumber(damage);
        Debug.Log($"Blacksmith took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            isDead = true;
            currentHealth = 0;
            if (onDeath != null)
                onDeath.Invoke();
            if (gameOverPanel != null)
                gameOverPanel.SetActive(true);
            Die();
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

    void Die()
    {
        Debug.Log("Blacksmith has died!");
        // Add any death effects or game over logic here
    }

    public bool IsAlive()
    {
        return !isDead;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}