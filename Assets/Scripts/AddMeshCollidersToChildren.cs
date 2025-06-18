using UnityEngine;

public class AddMeshCollidersToChildren : MonoBehaviour
{
    [ContextMenu("Add Mesh Colliders To All Children")]
    void AddColliders()
    {
        foreach (var mf in GetComponentsInChildren<MeshFilter>())
        {
            if (mf.GetComponent<MeshCollider>() == null)
            {
                var mc = mf.gameObject.AddComponent<MeshCollider>();
                mc.convex = false; // For static objects
            }
        }
        Debug.Log("Added MeshColliders to all children!");
    }
}