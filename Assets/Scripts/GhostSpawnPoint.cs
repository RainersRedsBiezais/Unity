using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GhostSpawnPoint : MonoBehaviour
{
    private const string SPAWN_POINT_TAG = "GhostSpawnPoint";

    void Reset()
    {
        // This is called when the component is first added or reset
        #if UNITY_EDITOR
        // Create tag if it doesn't exist
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        bool found = false;
        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(SPAWN_POINT_TAG)) { found = true; break; }
        }

        if (!found)
        {
            tagsProp.InsertArrayElementAtIndex(tagsProp.arraySize);
            SerializedProperty newTag = tagsProp.GetArrayElementAtIndex(tagsProp.arraySize - 1);
            newTag.stringValue = SPAWN_POINT_TAG;
            tagManager.ApplyModifiedProperties();
            Debug.Log($"Created tag: {SPAWN_POINT_TAG}");
        }
        #endif

        // Set the tag
        gameObject.tag = SPAWN_POINT_TAG;
    }

    void Awake()
    {
        // Ensure this object has the correct tag
        if (gameObject.tag != SPAWN_POINT_TAG)
        {
            gameObject.tag = SPAWN_POINT_TAG;
            Debug.Log($"Updated tag for spawn point: {gameObject.name}");
        }
    }

    void OnDrawGizmos()
    {
        // Visual indicator in scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);
        Gizmos.DrawRay(transform.position, transform.forward * 2f);
    }
} 