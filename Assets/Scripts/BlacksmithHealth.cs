using UnityEngine;

public class BlacksmithHealth : MonoBehaviour
{
    public int maxHealth = 200;
    private int currentHealth;

    public delegate void BlacksmithDestroyed();
    public event BlacksmithDestroyed OnBlacksmithDestroyed;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            if (OnBlacksmithDestroyed != null)
                OnBlacksmithDestroyed();
            Destroy(gameObject);
        }
    }
} 