using UnityEngine;
using UnityEngine.Events;

public class BlacksmithHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public UnityEvent onDeath;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            if (onDeath != null)
                onDeath.Invoke();
        }
    }
}