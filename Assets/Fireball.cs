using UnityEngine;

public class Fireball : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 20f;
    
    [Header("Explosion Effect")]
    public GameObject explosionEffectPrefab;
    public float explosionEffectDuration = 2f;
    
    [Header("Damage Settings")]
    public int damage = 999; // Ensure one-shot kill
    public float damageRadius = 5f; // Increased radius to make hitting easier
    public LayerMask damageLayers;

    private Rigidbody rb;
    private bool hasExploded = false;

    void Start()
    {
        Debug.Log($"Fireball spawned at {transform.position}");
        
        // Make sure we have a Rigidbody for physics
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = false;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }

        // Make sure we have a collider
        SphereCollider col = GetComponent<SphereCollider>();
        if (col == null)
        {
            col = gameObject.AddComponent<SphereCollider>();
            col.isTrigger = true;
            col.radius = 1f; // Increased radius to make hitting easier
        }

        // Set initial velocity
        rb.linearVelocity = transform.forward * speed;
        Debug.Log($"Fireball initial velocity: {rb.linearVelocity}");

        // Set the damage layers to include Enemy layer (layer 6)
        damageLayers = 1 << 6; // This sets it to only hit layer 6 (Enemy)
        
        // Destroy after 5 seconds if nothing is hit
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        // Check for nearby enemies each frame
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageRadius, damageLayers);
        if (hitColliders.Length > 0 && !hasExploded)
        {
            Debug.Log($"Found {hitColliders.Length} enemies in range during Update");
            Explode();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Fireball triggered with: {other.gameObject.name} on layer {other.gameObject.layer} at position {transform.position}");
        
        // Check if we hit something on the Enemy layer
        if (((1 << other.gameObject.layer) & damageLayers.value) != 0)
        {
            if (!hasExploded)
            {
                Debug.Log($"Hit valid target on layer {other.gameObject.layer}, exploding!");
                Explode();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Fireball collided with: {collision.gameObject.name} on layer {collision.gameObject.layer} at position {transform.position}");
        
        // Check if we hit something on the Enemy layer
        if (((1 << collision.gameObject.layer) & damageLayers.value) != 0)
        {
            if (!hasExploded)
            {
                Debug.Log($"Hit valid target on layer {collision.gameObject.layer}, exploding!");
                Explode();
            }
        }
    }

    void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;
        
        Debug.Log($"Fireball exploding at {transform.position} with damage radius {damageRadius}");
        
        // Deal damage to nearby enemies
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageRadius, damageLayers);
        Debug.Log($"Found {hitColliders.Length} colliders in explosion radius");
        
        foreach (var hitCollider in hitColliders)
        {
            Debug.Log($"Checking collider: {hitCollider.name} on layer {LayerMask.LayerToName(hitCollider.gameObject.layer)}");
            GhostHealth ghostHealth = hitCollider.GetComponent<GhostHealth>();
            if (ghostHealth != null)
            {
                Debug.Log($"Dealing {damage} damage to {hitCollider.name}");
                ghostHealth.TakeDamage(damage);
            }
            else
            {
                Debug.Log($"No GhostHealth component found on {hitCollider.name}");
                // Try getting the component in parent
                ghostHealth = hitCollider.GetComponentInParent<GhostHealth>();
                if (ghostHealth != null)
                {
                    Debug.Log($"Found GhostHealth in parent, dealing {damage} damage to {hitCollider.name}");
                    ghostHealth.TakeDamage(damage);
                }
            }
        }

        // Spawn explosion effect
        if (explosionEffectPrefab != null)
        {
            GameObject explosion = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, explosionEffectDuration);
        }

        // Destroy the fireball
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the damage radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
} 