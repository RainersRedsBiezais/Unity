using UnityEngine;
using System.Collections.Generic;

public class TreePlacer : MonoBehaviour
{
    [Header("Tree Settings")]
    [SerializeField] private GameObject treePrefab;
    [SerializeField] private int numberOfTrees = 50;
    
    [Header("Placement Area")]
    [SerializeField] private Vector3 areaSize = new Vector3(100f, 0f, 100f);
    [SerializeField] private LayerMask groundLayer = 1;
    [SerializeField] private float maxSlope = 30f;
    [SerializeField] private float groundHeight = 0f; // Fallback height if no ground found
    
    [Header("Clear Area (No Trees)")]
    public Transform clearCenter;
    public float clearRadius = 10f;
    
    [Header("Border Wall")]
    public float borderThickness = 5f; // in meters
    public float borderSpacing = 2.5f; // spacing between border trees
    
    [Header("Tree Variations")]
    [SerializeField] private Vector2 scaleRange = new Vector2(0.8f, 1.2f);
    [SerializeField] private float rotationVariation = 360f;
    [SerializeField] private float minDistanceBetweenTrees = 3f;
    
    [Header("Collision Settings")]
    [SerializeField] private bool useCollisionDetection = true;
    [SerializeField] private LayerMask collisionLayer = 1;
    
    [Header("Debug")]
    [SerializeField] private bool showDebugInfo = true;
    
    private List<GameObject> placedTrees = new List<GameObject>();
    
    [ContextMenu("Place Trees")]
    public void PlaceTrees()
    {
        if (treePrefab == null)
        {
            Debug.LogError("Tree prefab is not assigned! Please assign a tree prefab in the inspector.");
            return;
        }
        
        ClearTrees();
        
        // 1. Place border wall of trees in a grid
        int borderTreesPlaced = PlaceBorderWall();
        
        // 2. Place interior trees randomly, avoiding clear area and border
        int attempts = 0;
        int maxAttempts = numberOfTrees * 20;
        int normalTreesPlaced = 0;
        for (int i = 0; i < numberOfTrees && attempts < maxAttempts; i++)
        {
            attempts++;
            Vector3 randomPosition = GetRandomPosition();
            if (IsInBorderWall(randomPosition) || IsInClearArea(randomPosition))
                continue;
            if (IsValidPosition(randomPosition))
            {
                PlaceTree(randomPosition);
                normalTreesPlaced++;
                if (showDebugInfo)
                    Debug.Log($"Placed interior tree {placedTrees.Count} at position: {randomPosition}");
            }
        }
        
        Debug.Log($"Placed {placedTrees.Count} trees successfully! (Border: {borderTreesPlaced}, Normal: {normalTreesPlaced}, Attempts: {attempts})");
        
        if (placedTrees.Count == 0)
        {
            Debug.LogWarning("No trees were placed! Check the following:");
            Debug.LogWarning("1. Is the tree prefab assigned?");
            Debug.LogWarning("2. Is there ground/terrain in the placement area?");
            Debug.LogWarning("3. Are the ground layer settings correct?");
            Debug.LogWarning("4. Is the placement area size appropriate?");
        }
    }
    
    // Place a thick border wall of trees in a grid around the edge
    private int PlaceBorderWall()
    {
        int count = 0;
        Vector3 center = transform.position;
        Vector3 halfSize = areaSize * 0.5f;
        float minX = center.x - halfSize.x;
        float maxX = center.x + halfSize.x;
        float minZ = center.z - halfSize.z;
        float maxZ = center.z + halfSize.z;
        float innerMinX = minX + borderThickness;
        float innerMaxX = maxX - borderThickness;
        float innerMinZ = minZ + borderThickness;
        float innerMaxZ = maxZ - borderThickness;
        
        // Place trees along the border in a grid
        for (float x = minX; x <= maxX; x += borderSpacing)
        {
            for (float z = minZ; z <= maxZ; z += borderSpacing)
            {
                bool isBorder = (x <= innerMinX || x >= innerMaxX || z <= innerMinZ || z >= innerMaxZ);
                if (isBorder)
                {
                    Vector3 pos = new Vector3(x, 100f, z);
                    // Raycast down to find ground
                    if (Physics.Raycast(pos, Vector3.down, out RaycastHit hit, 200f, groundLayer))
                        pos = hit.point;
                    else
                        pos = new Vector3(x, groundHeight, z);
                    if (IsInClearArea(pos))
                        continue;
                    PlaceTree(pos);
                    count++;
                    if (showDebugInfo)
                        Debug.Log($"Placed border tree at {pos}");
                }
            }
        }
        return count;
    }
    
    // Check if a position is within the border wall area
    private bool IsInBorderWall(Vector3 position)
    {
        Vector3 center = transform.position;
        Vector3 halfSize = areaSize * 0.5f;
        float minX = center.x - halfSize.x;
        float maxX = center.x + halfSize.x;
        float minZ = center.z - halfSize.z;
        float maxZ = center.z + halfSize.z;
        float innerMinX = minX + borderThickness;
        float innerMaxX = maxX - borderThickness;
        float innerMinZ = minZ + borderThickness;
        float innerMaxZ = maxZ - borderThickness;
        float x = position.x;
        float z = position.z;
        return (x <= innerMinX || x >= innerMaxX || z <= innerMinZ || z >= innerMaxZ);
    }
    
    // Check if a position is within the clear area (cylindrical, XZ only)
    private bool IsInClearArea(Vector3 position)
    {
        if (clearCenter == null) return false;
        Vector2 posXZ = new Vector2(position.x, position.z);
        Vector2 centerXZ = new Vector2(clearCenter.position.x, clearCenter.position.z);
        float dist = Vector2.Distance(posXZ, centerXZ);
        return dist < clearRadius;
    }
    
    [ContextMenu("Clear Trees")]
    public void ClearTrees()
    {
        foreach (GameObject tree in placedTrees)
        {
            if (tree != null)
            {
                if (Application.isPlaying)
                    Destroy(tree);
                else
                    DestroyImmediate(tree);
            }
        }
        placedTrees.Clear();
        Debug.Log("All trees cleared!");
    }
    
    private Vector3 GetRandomPosition()
    {
        Vector3 center = transform.position;
        Vector3 halfSize = areaSize * 0.5f;
        float x = Random.Range(-halfSize.x, halfSize.x);
        float z = Random.Range(-halfSize.z, halfSize.z);
        Vector3 randomPos = center + new Vector3(x, 100f, z);
        // Raycast down to find ground
        if (Physics.Raycast(randomPos, Vector3.down, out RaycastHit hit, 200f, groundLayer))
        {
            if (showDebugInfo)
                Debug.DrawRay(randomPos, Vector3.down * 200f, Color.green, 2f);
            return hit.point;
        }
        else
        {
            if (showDebugInfo)
            {
                Debug.DrawRay(randomPos, Vector3.down * 200f, Color.red, 2f);
                Debug.LogWarning($"No ground found at {randomPos}, using fallback height: {groundHeight}");
            }
            return new Vector3(randomPos.x, groundHeight, randomPos.z);
        }
    }
    
    private bool IsValidPosition(Vector3 position)
    {
        // 1. Check slope
        if (Physics.Raycast(position + Vector3.up, Vector3.down, out RaycastHit hit, 2f, groundLayer))
        {
            float slope = Vector3.Angle(hit.normal, Vector3.up);
            if (slope > maxSlope)
            {
                if (showDebugInfo)
                    Debug.Log($"Position rejected: slope too steep ({slope} degrees)");
                return false;
            }
        }
        // 2. Check distance from other trees
        foreach (GameObject tree in placedTrees)
        {
            if (tree != null && Vector3.Distance(position, tree.transform.position) < minDistanceBetweenTrees)
            {
                if (showDebugInfo)
                    Debug.Log($"Position rejected: too close to existing tree");
                return false;
            }
        }
        // 3. Check for existing collisions
        if (useCollisionDetection)
        {
            Collider[] colliders = Physics.OverlapSphere(position, minDistanceBetweenTrees * 0.5f, collisionLayer);
            if (colliders.Length > 0)
            {
                if (showDebugInfo)
                    Debug.Log($"Position rejected: collision detected with {colliders.Length} objects");
                return false;
            }
        }
        return true;
    }
    
    private void PlaceTree(Vector3 position)
    {
        if (treePrefab == null)
        {
            Debug.LogError("Tree prefab is not assigned!");
            return;
        }
        GameObject tree = Instantiate(treePrefab, position, Quaternion.identity, transform);
        // Apply random rotation
        float randomRotation = Random.Range(0f, rotationVariation);
        tree.transform.rotation = Quaternion.Euler(0f, randomRotation, 0f);
        // Apply random scale
        float randomScale = Random.Range(scaleRange.x, scaleRange.y);
        tree.transform.localScale = Vector3.one * randomScale;
        placedTrees.Add(tree);
    }
    
    private void OnDrawGizmosSelected()
    {
        // Draw placement area
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, areaSize);
        // Draw clear area
        if (clearCenter != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(clearCenter.position, clearRadius);
        }
        // Draw border
        Gizmos.color = Color.blue;
        Vector3 center = transform.position;
        Vector3 halfSize = areaSize * 0.5f;
        float border = borderThickness;
        Gizmos.DrawWireCube(center, new Vector3(areaSize.x - 2 * border, 0.1f, areaSize.z - 2 * border));
        Gizmos.DrawWireCube(center, new Vector3(areaSize.x, 0.1f, areaSize.z));
        // Draw placed trees
        Gizmos.color = Color.yellow;
        foreach (GameObject tree in placedTrees)
        {
            if (tree != null)
                Gizmos.DrawWireSphere(tree.transform.position, minDistanceBetweenTrees * 0.5f);
        }
        // Draw ground detection rays
        if (showDebugInfo)
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < 5; i++)
            {
                float x = Random.Range(-halfSize.x, halfSize.x);
                float z = Random.Range(-halfSize.z, halfSize.z);
                Vector3 rayStart = center + new Vector3(x, 100f, z);
                Gizmos.DrawRay(rayStart, Vector3.down * 200f);
            }
        }
    }
    
    // Public method to place a single tree at a specific position
    public void PlaceTreeAtPosition(Vector3 position)
    {
        if (IsValidPosition(position))
        {
            PlaceTree(position);
        }
        else
        {
            Debug.LogWarning("Invalid position for tree placement!");
        }
    }
    
    // Public method to get the number of currently placed trees
    public int GetTreeCount()
    {
        return placedTrees.Count;
    }
    
    // Debug method to test ground detection
    [ContextMenu("Test Ground Detection")]
    public void TestGroundDetection()
    {
        Debug.Log("Testing ground detection...");
        Vector3 testPosition = GetRandomPosition();
        Debug.Log($"Test position: {testPosition}");
        bool isValid = IsValidPosition(testPosition);
        Debug.Log($"Position valid: {isValid}");
    }
} 