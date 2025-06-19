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
    public float castPointDistance = 1.5f;

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
            Debug.Log("Creating new cast point");
            GameObject castPointObj = new GameObject("CastPoint");
            castPoint = castPointObj.transform;
            castPoint.parent = playerCamera.transform;
            castPoint.localPosition = new Vector3(0, -0.2f, castPointDistance);
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

        if (fireballPrefab == null)
        {
            Debug.LogError("Fireball prefab is not assigned!");
        }
        if (flamethrowerPrefab == null)
        {
            Debug.LogError("Flamethrower prefab is not assigned!");
        }

        // Initialize spell prefabs array
        spellPrefabs = new GameObject[] { fireballPrefab, flamethrowerPrefab };
        Debug.Log($"Initialized with {spellPrefabs.Length} spells");
    }

    void Update()
    {
        // Spell selection (keys 1-2)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentSpellIndex = 0;
            Debug.Log("Switched to Fireball");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentSpellIndex = 1;
            Debug.Log("Switched to Flamethrower");
        }
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
            Debug.LogError($"Selected spell prefab ({currentSpellIndex}) is not assigned!");
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
        Vector3 mouthOffset = playerCamera.transform.up * -0.1f;
        Vector3 spawnPosition = playerCamera.transform.position + playerCamera.transform.forward * castPointDistance + mouthOffset;
        Quaternion spawnRotation = Quaternion.LookRotation(playerCamera.transform.forward);
        Debug.Log($"Attempting to instantiate spell at position: {spawnPosition}");
        
        GameObject spell = Instantiate(spellPrefabs[currentSpellIndex], spawnPosition, spawnRotation);
        if (spell == null)
        {
            Debug.LogError("Failed to instantiate spell!");
        }
        else
        {
            Debug.Log($"Spell {spellPrefabs[currentSpellIndex].name} instantiated successfully");
            Rigidbody rb = spell.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = playerCamera.transform.forward * fireballSpeed;
                Debug.Log($"Set spell velocity to {rb.linearVelocity}");
            }
        }

        // Cooldown
        yield return new WaitForSeconds(cooldownDuration);
        canCast = true;
        Debug.Log("Spell cooldown finished");
    }

    public int GetCurrentSpellIndex()
    {
        return currentSpellIndex;
    }
}