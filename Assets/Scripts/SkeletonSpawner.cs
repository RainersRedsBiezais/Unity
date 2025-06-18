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
    Vector2 circle = Random.insideUnitCircle.normalized * spawnRadius;
    Vector3 spawnPos = new Vector3(circle.x, 20, circle.y) + target.position; // Start high above the ground

    RaycastHit hit;
    if (Physics.Raycast(spawnPos, Vector3.down, out hit, 100f, groundLayer))
    {
        spawnPos = hit.point;
        GameObject skeleton = Instantiate(skeletonPrefab, spawnPos, Quaternion.identity);
        SkeletonAI ai = skeleton.GetComponent<SkeletonAI>();
        if (ai != null)
        {
            ai.SetTarget(target);
        }
        currentSkeletons++;
    }
    // else: did not hit ground, do not spawn
    }

    public void OnSkeletonDestroyed()
    {
        currentSkeletons = Mathf.Max(0, currentSkeletons - 1);
    }
} 