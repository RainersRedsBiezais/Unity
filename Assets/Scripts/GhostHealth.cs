using UnityEngine;

public class GhostHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        Debug.Log($"GhostHealth Start() called on {gameObject.name}");
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError($"No Animator component found on {gameObject.name}");
        }
        Debug.Log($"Ghost {gameObject.name} initialized with {currentHealth} HP");
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        
        Debug.Log($"Ghost {gameObject.name} taking {damage} damage. Current health: {currentHealth}");
        isDead = true;
        Die();
    }

    void Die()
    {
        Debug.Log($"Ghost {gameObject.name} died");
        
        // Disable all components that might interfere with death
        var collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;
        
        var ghostAI = GetComponent<GhostAI>();
        if (ghostAI != null) ghostAI.enabled = false;
        
        var navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (navMeshAgent != null) navMeshAgent.enabled = false;

        if (animator != null)
        {
            Debug.Log($"Playing death animation on {gameObject.name}");
            animator.SetTrigger("Die");
            // Destroy immediately after starting the animation
            Destroy(gameObject, 0.1f);
        }
        else
        {
            Debug.LogError($"No animator found on {gameObject.name} for death animation");
            // If no animator, destroy immediately
            Destroy(gameObject);
        }
    }

    // Test method to manually trigger damage
    public void TestTakeDamage(int amount)
    {
        Debug.Log($"Test damage called on {gameObject.name} with amount {amount}");
        TakeDamage(amount);
    }
} 