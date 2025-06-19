using UnityEngine;
using UnityEngine.AI;

public class GhostAI : MonoBehaviour
{
    public Transform target;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    private NavMeshAgent agent;
    private Animator animator;
    private float nextAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (target == null)
        {
            var blacksmith = FindFirstObjectByType<BlacksmithHealth>();
            if (blacksmith != null)
            {
                target = blacksmith.transform;
            }
            else
            {
                Debug.LogError("No target found for ghost to chase!");
            }
        }
    }

    void Update()
    {
        if (target == null || agent == null) return;

        // Update destination
        agent.SetDestination(target.position);

        // Check if in attack range
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget <= attackRange && Time.time >= nextAttackTime)
        {
            Attack();
        }

        // Update animation
        if (animator != null)
        {
            animator.SetBool("isMoving", agent.velocity.magnitude > 0.1f);
        }
    }

    void Attack()
    {
        nextAttackTime = Time.time + attackCooldown;
        
        if (animator != null)
        {
            animator.SetTrigger("attack");
        }

        // Deal damage to target
        BlacksmithHealth blacksmith = target.GetComponent<BlacksmithHealth>();
        if (blacksmith != null)
        {
            blacksmith.TakeDamage(10);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}