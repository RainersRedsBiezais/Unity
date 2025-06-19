using UnityEngine;
using System.Collections;

public class SpellCaster : MonoBehaviour
{
    public GameObject fireballPrefab;
    public GameObject flamethrowerPrefab;
    public Transform castPoint;
    public GameObject vapeModel;
    public float vapeDuration = 0.5f;
    public float cooldownDuration = 1f;
    public float vapeIntensity = 0.3f;
    public float fireballSpeed = 20f;
    public float castPointDistance = 1.5f; // Increased distance in front of the player

    private bool canCast = true;
    private Vector3 originalVapePosition;
    private Quaternion originalVapeRotation;
    private Camera playerCamera;
    private GameObject[] spellPrefabs;
    private int currentSpellIndex = 0;

    void Start()
    {
        Debug.Log("SpellCaster initialized");
        playerCamera = GetComponentInChildren<Camera>();
        
        if (playerCamera == null)
        {
            Debug.LogError("No camera found in children of player!");
            return;
        }

        // Create cast point if it doesn't exist
        if (castPoint == null)
        {
            GameObject castPointObj = new GameObject("CastPoint");
            castPoint = castPointObj.transform;
            castPoint.parent = playerCamera.transform;
            castPoint.localPosition = new Vector3(0, -0.2f, castPointDistance); // Position slightly below camera center
            castPoint.localRotation = Quaternion.identity;
        }

        if (vapeModel != null)
        {
            originalVapePosition = vapeModel.transform.localPosition;
            originalVapeRotation = vapeModel.transform.localRotation;
        }
        else
        {
            Debug.LogWarning("Vape model is not assigned!");
        }

        if (fireballPrefab == null || flamethrowerPrefab == null)
        {
            Debug.LogError("Spell prefabs are not assigned!");
        }

        // Initialize spell prefabs array
        spellPrefabs = new GameObject[2];
        spellPrefabs[0] = fireballPrefab;
        spellPrefabs[1] = flamethrowerPrefab;
    }

    void Update()
    {
        // Spell selection (keys 1-2)
        if (Input.GetKeyDown(KeyCode.Alpha1)) currentSpellIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) currentSpellIndex = 1;
        currentSpellIndex = Mathf.Clamp(currentSpellIndex, 0, spellPrefabs.Length - 1);

        if (Input.GetMouseButtonDown(0) && canCast)
        {
            Debug.Log("Mouse button pressed, attempting to cast spell");
            StartCoroutine(VapeAndCast());
        }
    }

    IEnumerator VapeAndCast()
    {
        Debug.Log("Starting VapeAndCast coroutine");
        canCast = false;

        // Defensive null checks
        if (playerCamera == null)
        {
            Debug.LogError("Player camera is not assigned!");
            canCast = true;
            yield break;
        }
        if (spellPrefabs == null || spellPrefabs.Length == 0)
        {
            Debug.LogError("Spell prefabs array is not initialized!");
            canCast = true;
            yield break;
        }
        if (currentSpellIndex < 0 || currentSpellIndex >= spellPrefabs.Length || spellPrefabs[currentSpellIndex] == null)
        {
            Debug.LogError("Selected spell prefab is not assigned!");
            canCast = true;
            yield break;
        }

        // Simple vaping animation: move vape up and rotate slightly
        if (vapeModel != null)
        {
            Vector3 vapeUpPosition = originalVapePosition + Vector3.up * vapeIntensity;
            float elapsedTime = 0f;

            while (elapsedTime < vapeDuration)
            {
                float t = elapsedTime / vapeDuration;
                vapeModel.transform.localPosition = Vector3.Lerp(originalVapePosition, vapeUpPosition, t);
                vapeModel.transform.localRotation = Quaternion.Lerp(originalVapeRotation, 
                    originalVapeRotation * Quaternion.Euler(0, 0, 15f), t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Return vape to original position
            elapsedTime = 0f;
            while (elapsedTime < vapeDuration)
            {
                float t = elapsedTime / vapeDuration;
                vapeModel.transform.localPosition = Vector3.Lerp(vapeUpPosition, originalVapePosition, t);
                vapeModel.transform.localRotation = Quaternion.Lerp(
                    originalVapeRotation * Quaternion.Euler(0, 0, 15f), 
                    originalVapeRotation, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        // Cast selected spell from camera's mouth position in the direction the player is looking
        Vector3 mouthOffset = playerCamera.transform.up * -0.1f; // Reduced downward offset
        Vector3 spawnPosition = playerCamera.transform.position + playerCamera.transform.forward * castPointDistance + mouthOffset;
        Quaternion spawnRotation = Quaternion.LookRotation(playerCamera.transform.forward);
        Debug.Log("Attempting to instantiate spell at position: " + spawnPosition);
        GameObject spell = Instantiate(spellPrefabs[currentSpellIndex], spawnPosition, spawnRotation);
        if (spell == null)
        {
            Debug.LogError("Failed to instantiate spell!");
        }
        else
        {
            Debug.Log("Spell instantiated successfully");
            // No need to set Rigidbody velocity; let the prefab handle its own movement
        }

        // Cooldown
        yield return new WaitForSeconds(cooldownDuration);
        canCast = true;
        Debug.Log("Spell cooldown finished");
    }

    // Public method to get current spell index for UI
    public int GetCurrentSpellIndex()
    {
        return currentSpellIndex;
    }
}