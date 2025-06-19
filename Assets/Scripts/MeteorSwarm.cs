using UnityEngine;

public class MeteorSwarm : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage = 60;
    public float damageRadius = 5f;
    public LayerMask damageLayers;
    public float meteorLifetime = 5f;

    [Header("Effects")]
    public GameObject impactEffectPrefab;
    public float effectDuration = 2f;

    void Start()
    {
        Destroy(gameObject, meteorLifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Impact();
    }

    void Impact()
    {
        // Deal damage to nearby enemies
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageRadius, damageLayers);
        foreach (var hitCollider in hitColliders)
        {
            GhostHealth ghostHealth = hitCollider.GetComponent<GhostHealth>();
            if (ghostHealth != null)
            {
                ghostHealth.TakeDamage(damage);
            }
        }

        // Spawn impact effect
        if (impactEffectPrefab != null)
        {
            GameObject impact = Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
            Destroy(impact, effectDuration);
        }

        // Destroy the meteor
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the damage radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
} 