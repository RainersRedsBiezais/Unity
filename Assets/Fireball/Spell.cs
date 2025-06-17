using UnityEngine;

public class Spell : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 10f;
    public int damage = 10;
    public GameObject impactEffect;
    public float impactRadius = 1f;
    public LayerMask collisionLayers;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Don't destroy on trigger with the player or other spells
        if (other.CompareTag("Player") || other.CompareTag("Spell")) return;

        // Only destroy if we hit something on the specified layers
        if (collisionLayers == (collisionLayers | (1 << other.gameObject.layer)))
        {
            // Create impact effect if assigned
            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, Quaternion.identity);
            }

            // Check for damageable objects in radius
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, impactRadius);
            foreach (var hitCollider in hitColliders)
            {
                // You can add your damage interface here later
                Debug.Log($"Hit: {hitCollider.name}");
            }

            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, impactRadius);
    }
}
