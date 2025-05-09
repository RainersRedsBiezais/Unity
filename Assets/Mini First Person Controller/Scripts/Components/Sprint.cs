using UnityEngine;

public class Sprint : MonoBehaviour
{
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Sprint Settings")]
    [Tooltip("Movement to speed up when sprinting.")]
    public FirstPersonMovement movement;
    [Tooltip("Crouch component to check if player is crouched.")]
    public Crouch crouch;

    [Tooltip("Movement speed when sprinting.")]
    public float sprintSpeed = 10f;

    private bool isSprinting = false;

    void Reset()
    {
        // Try to get the FirstPersonMovement component.
        movement = GetComponentInParent<FirstPersonMovement>();
        crouch = GetComponentInParent<Crouch>();
    }

    void OnEnable()
    {
        if (crouch != null)
        {
            crouch.CrouchStart += OnCrouchStart;
            crouch.CrouchEnd += OnCrouchEnd;
        }
    }

    void OnDisable()
    {
        if (crouch != null)
        {
            crouch.CrouchStart -= OnCrouchStart;
            crouch.CrouchEnd -= OnCrouchEnd;
        }
    }

    void OnCrouchStart()
    {
        // Force stop sprinting when crouching starts
        if (isSprinting)
        {
            StopSprinting();
        }
    }

    void OnCrouchEnd()
    {
        // Don't automatically start sprinting when crouch ends
        // Let the normal sprint input handling take care of it
    }

    void Update()
    {
        // Don't process sprint input if crouched
        if (crouch != null && crouch.IsCrouched)
        {
            return;
        }

        // Handle sprint input only when not crouched
        if (Input.GetKey(sprintKey))
        {
            if (!isSprinting)
            {
                StartSprinting();
            }
        }
        else if (isSprinting)
        {
            StopSprinting();
        }
    }

    void StartSprinting()
    {
        if (!isSprinting && movement && (crouch == null || !crouch.IsCrouched))
        {
            // Remove any existing speed overrides first
            if (movement.speedOverrides.Count > 0)
            {
                movement.speedOverrides.Clear();
            }
            movement.speedOverrides.Add(SprintSpeedOverride);
            isSprinting = true;
        }
    }

    void StopSprinting()
    {
        if (isSprinting && movement)
        {
            movement.speedOverrides.Remove(SprintSpeedOverride);
            isSprinting = false;
        }
    }

    float SprintSpeedOverride() => sprintSpeed;
}
