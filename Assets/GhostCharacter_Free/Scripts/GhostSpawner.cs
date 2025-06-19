using UnityEngine;
using System.Collections;

public class GhostSpawner : MonoBehaviour
{
    public GameObject ghostPrefab;
    public float spawnInterval = 5f;
    public float minSpawnDistance = 20f; // Increased minimum distance from blacksmith
    public float maxSpawnDistance = 30f; // Increased maximum spawn distance
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

        // Get random angle
        float randomAngle = Random.Range(0f, 360f);
        // Get random distance between min and max
        float randomDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
        
        // Convert polar coordinates to Cartesian
        float x = Mathf.Cos(randomAngle * Mathf.Deg2Rad) * randomDistance;
        float z = Mathf.Sin(randomAngle * Mathf.Deg2Rad) * randomDistance;
        
        Vector3 spawnPosition = target.position + new Vector3(x, 0, z);

        // Check if spawn position is valid (you might want to add NavMesh checking here)
        RaycastHit hit;
        if (Physics.Raycast(spawnPosition + Vector3.up * 10f, Vector3.down, out hit, 20f))
        {
            spawnPosition.y = hit.point.y;
        }

        // Instantiate ghost
        GameObject ghost = Instantiate(ghostPrefab, spawnPosition, Quaternion.identity);
        
        // Set up ghost AI
        GhostAI ghostAI = ghost.GetComponent<GhostAI>();
        if (ghostAI != null)
        {
            ghostAI.target = target;
        }
        
        Debug.Log($"Spawned ghost at distance {Vector3.Distance(spawnPosition, target.position)} from target");
    }

    void OnDrawGizmosSelected()
    {
        if (target != null)
        {
            // Draw min spawn radius
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(target.position, minSpawnDistance);
            
            // Draw max spawn radius
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(target.position, maxSpawnDistance);
        }
    }
}