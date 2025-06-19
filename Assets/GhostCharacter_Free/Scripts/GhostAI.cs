using UnityEngine;
using UnityEngine.AI;

public class GhostAI : MonoBehaviour
{
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime;
    private Transform target;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GameObject blacksmith = GameObject.FindGameObjectWithTag("Blacksmith");
        if (blacksmith != null)
        {
            target = blacksmith.transform;
        }
    }

    void Update()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > attackRange)
        {
            // Move towards the target
            agent.isStopped = false;
            agent.SetDestination(target.position);
        }
        else
        {
            // Attack
            agent.isStopped = true;
            if (Time.time - lastAttackTime > attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }

    void Attack()
{
    if (target != null)
    {
        BlacksmithHealth health = target.GetComponent<BlacksmithHealth>();
        if (health != null)
        {
            health.TakeDamage(10); // Deal 10 damage per attack
        }
    }
}
}