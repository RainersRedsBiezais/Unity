using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TreeCollision : MonoBehaviour
{
    [Header("Collision Settings")]
    [SerializeField] private bool isSolid = true;
    [SerializeField] private bool useRigidbody = false;
    [SerializeField] private float collisionRadius = 1f;
    
    private Collider treeCollider;
    private Rigidbody treeRigidbody;
    
    void Awake()
    {
        // Get or add required components
        treeCollider = GetComponent<Collider>();
        if (treeCollider == null)
        {
            // Add a capsule collider if none exists
            CapsuleCollider capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.radius = collisionRadius;
            capsuleCollider.height = 5f; // Adjust based on your tree height
            capsuleCollider.center = new Vector3(0, 2.5f, 0); // Center at tree trunk
            treeCollider = capsuleCollider;
        }
        
        // Ensure collider is not a trigger
        treeCollider.isTrigger = false;
        
        // Add rigidbody if needed for physics interactions
        if (useRigidbody)
        {
            treeRigidbody = GetComponent<Rigidbody>();
            if (treeRigidbody == null)
            {
                treeRigidbody = gameObject.AddComponent<Rigidbody>();
                treeRigidbody.isKinematic = true; // Trees shouldn't move
                treeRigidbody.useGravity = false;
            }
        }
        
        // Set layer to ensure proper collision detection
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
    
    void OnValidate()
    {
        // Update collision radius in editor
        if (treeCollider != null && treeCollider is CapsuleCollider)
        {
            CapsuleCollider capsule = treeCollider as CapsuleCollider;
            capsule.radius = collisionRadius;
        }
    }
    
    // Method to check if a position is blocked by this tree
    public bool IsPositionBlocked(Vector3 position)
    {
        if (!isSolid) return false;
        
        float distance = Vector3.Distance(transform.position, position);
        return distance < collisionRadius;
    }
    
    // Method to get the closest valid position around the tree
    public Vector3 GetClosestValidPosition(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        return transform.position + direction * collisionRadius;
    }
    
    // Optional: Add visual debugging
    void OnDrawGizmosSelected()
    {
        Gizmos.color = isSolid ? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, collisionRadius);
    }
} 