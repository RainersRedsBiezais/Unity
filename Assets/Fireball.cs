using UnityEngine;

public class Fireball : MonoBehaviour
{
    [Header("Explosion Effect")]
    public GameObject explosionEffectPrefab; // Assign a particle system prefab in the inspector
    public float explosionEffectDuration = 2f; // How long the explosion effect stays

    void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    void Explode()
    {
        // Spawn explosion effect
        if (explosionEffectPrefab != null)
        {
            GameObject explosion = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, explosionEffectDuration);
        }

        // Destroy the fireball
        Destroy(gameObject);
    }
} 