using UnityEngine;
using System.Collections;

public class GhostSpawner : MonoBehaviour
{
    public GameObject ghostPrefab;
    public float spawnInterval = 5f;
    public float spawnRadius = 10f;
    public Transform target;
    private bool isSpawning = true;

    void Start()
    {
        if (target == null)
        {
            // Find the blacksmith if target is not set
            var blacksmith = FindFirstObjectByType<BlacksmithHealth>();
            if (blacksmith != null)
            {
                target = blacksmith.transform;
            }
            else
            {
                Debug.LogError("No target found for ghosts to chase!");
                return;
            }
        }

        StartCoroutine(SpawnGhosts());
    }

    IEnumerator SpawnGhosts()
    {
        while (isSpawning)
        {
            SpawnGhost();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnGhost()
    {
        if (ghostPrefab == null || target == null) return;

        // Get random point in circle
        Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = new Vector3(randomPoint.x, 0, randomPoint.y) + target.position;

        // Instantiate ghost
        GameObject ghost = Instantiate(ghostPrefab, spawnPosition, Quaternion.identity);
        
        // Set up ghost AI
        GhostAI ghostAI = ghost.GetComponent<GhostAI>();
        if (ghostAI != null)
        {
            ghostAI.target = target;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (target != null)
        {
            // Draw spawn radius
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(target.position, spawnRadius);
        }
    }
}