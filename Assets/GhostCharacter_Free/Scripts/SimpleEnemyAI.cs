using UnityEngine;
using UnityEngine.AI;

public class SimpleEnemyAI : MonoBehaviour
{
    public Transform target; // Assign Blacksmith Place in Inspector or via spawner
    public float attackRange = 2f;
    public float attackInterval = 2f;
    public int attackDamage = 10;

    private NavMeshAgent agent;
    private float attackTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (target == null)
        {
            var obj = GameObject.FindGameObjectWithTag("Blacksmith");
            if (obj != null) target = obj.transform;
        }
    }

    void Update()
    {
        if (target == null || agent == null || !agent.isOnNavMesh) return;
        agent.SetDestination(target.position);

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= attackRange)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackInterval)
            {
                attackTimer = 0f;
                var health = target.GetComponent<BlacksmithHealth>();
                if (health != null)
                    health.TakeDamage(attackDamage);
            }
        }
        else
        {
            attackTimer = 0f;
        }
    }
} 