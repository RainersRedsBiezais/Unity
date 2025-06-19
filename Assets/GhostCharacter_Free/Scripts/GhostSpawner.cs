using UnityEngine;
using UnityEngine.AI;

public class GhostSpawner : MonoBehaviour
{
    public GameObject ghostPrefab;
    public int numberOfGhosts = 5;
    private GhostSpawnPoint[] spawnPoints;

    void Awake()
    {
        spawnPoints = FindObjectsOfType<GhostSpawnPoint>();
    }

    void Start()
    {
        for (int i = 0; i < numberOfGhosts; i++)
        {
            SpawnGhost();
        }
    }

    public void SpawnGhost()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No GhostSpawnPoints found in the scene!");
            return;
        }

        GhostSpawnPoint chosenPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Vector3 sourcePosition = chosenPoint.transform.position;
        NavMeshHit closestHit;

        // Try to find the nearest NavMesh point within 5 units
        if (NavMesh.SamplePosition(sourcePosition, out closestHit, 5.0f, NavMesh.AllAreas))
        {
            Instantiate(ghostPrefab, closestHit.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Could not find NavMesh close to spawn point: " + sourcePosition);
        }
    }
}