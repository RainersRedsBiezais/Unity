using UnityEngine;
using UnityEngine.AI;

public class SkeletonAI : MonoBehaviour
{
    public float attackRange = 2f;
    public float attackInterval = 2f;
    public int attackDamage = 10;
    public float moveSpeed = 3f;

    private Transform target;
    private float attackTimer = 0f;
    private NavMeshAgent agent;
    private Animator animator;
    private bool isAttacking = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > attackRange)
        {
            isAttacking = false;
            if (agent != null)
            {
                agent.isStopped = false;
                agent.SetDestination(target.position);
            }
            else
            {
                // Simple movement if no NavMeshAgent
                Vector3 dir = (target.position - transform.position).normalized;
                transform.position += dir * moveSpeed * Time.deltaTime;
            }
            if (animator != null) animator.SetBool("Attack", false);
        }
        else
        {
            if (!isAttacking)
            {
                isAttacking = true;
                if (agent != null) agent.isStopped = true;
                if (animator != null) animator.SetBool("Attack", true);
            }
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackInterval)
            {
                attackTimer = 0f;
                Attack();
            }
        }
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }

    void Attack()
    {
        // Call a method on the target to deal damage
        BlacksmithHealth health = target.GetComponent<BlacksmithHealth>();
        if (health != null)
        {
            health.TakeDamage(attackDamage);
        }
    }

    void OnDestroy()
    {
        SkeletonSpawner spawner = FindObjectOfType<SkeletonSpawner>();
        if (spawner != null)
        {
            spawner.OnSkeletonDestroyed();
        }
    }
} 