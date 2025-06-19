using UnityEngine;

public class SkeletonSpawner : MonoBehaviour
{
    public GameObject skeletonPrefab;
    public LayerMask groundLayer;
    public Transform target; // Assign Blacksmith Place here
    public float spawnRadius = 30f;
    public float spawnInterval = 5f;
    public int maxSkeletons = 10;

    private float timer = 0f;
    private int currentSkeletons = 0;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval && currentSkeletons < maxSkeletons)
        {
            timer = 0f;
            SpawnSkeleton();
        }
    }

    void SpawnSkeleton()
    {
        for (int attempts = 0; attempts < 10; attempts++) // Try up to 10 times to find a clear spot
        {
            Vector2 circle = Random.insideUnitCircle.normalized * spawnRadius;
            Vector3 spawnPos = new Vector3(circle.x, 20, circle.y) + target.position; // Start high above the ground

            RaycastHit hit;
            if (Physics.Raycast(spawnPos, Vector3.down, out hit, 100f, groundLayer))
            {
                spawnPos = hit.point;

                float checkRadius = 1.0f;
                float checkHeight = 4.0f;
                int steps = 4;
                bool blocked = false;
                for (int i = 0; i < steps; i++)
                {
                    Vector3 pos = spawnPos + Vector3.up * (i * (checkHeight / steps));
                    if (Physics.OverlapSphere(pos, checkRadius, LayerMask.GetMask("Tree")).Length > 0)
                    {
                        blocked = true;
                        Debug.Log("Blocked by tree at height: " + pos);
                        break;
                    }
                }
                if (!blocked)
                {
                    Instantiate(skeletonPrefab, spawnPos, Quaternion.identity);
                    currentSkeletons++;
                    return; // Successfully spawned, exit the function
                }
            }
        }
        // If we get here, we failed to find a clear spot after 10 tries
    }

    public void OnSkeletonDestroyed()
    {
        currentSkeletons = Mathf.Max(0, currentSkeletons - 1);
    }
} 