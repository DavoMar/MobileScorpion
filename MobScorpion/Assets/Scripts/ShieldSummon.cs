using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Required for UI interaction

// Spawns a shield hitbox when key or UI button is pressed, keeps it facing the right way, and then deletes it when released
public class ShieldSummon : MonoBehaviour
{
    public GameObject shieldPrefab; // Reference to the shield prefab
    public KeyCode shieldButton = KeyCode.LeftShift; // Keyboard button to activate the shield
    public Vector2 shieldOffset = new Vector2(0.5f, 0f); // Offset to position shield in front of player

    private GameObject shieldInstance; // Instance of the shield object
    private PlayerMovement playerMovement; // Reference to player movement to get direction

    private bool isShieldButtonHeld = false; // Tracks if UI button is held

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>(); // Get the player movement component
    }

    void Update()
    {
        // Check if either the keyboard button or UI button is being held
        if (Input.GetKey(shieldButton) || isShieldButtonHeld)
        {
            if (shieldInstance == null && !playerMovement.isAttacking)
            {
                ActivateShield();
            }
            UpdateShieldPosition();
        }
        else
        {
            if (shieldInstance != null)
            {
                DeactivateShield();
            }
        }
    }

    // Activates the shield
    void ActivateShield()
    {
        shieldInstance = Instantiate(shieldPrefab, transform.position + new Vector3(0f, 0.5f, 0f), Quaternion.identity);
        shieldInstance.transform.parent = transform; // Make the shield follow the player
        shieldInstance.GetComponent<Shield>().wielder = gameObject;
        playerMovement.isAttacking = true;
    }

    // Deactivates the shield
    void DeactivateShield()
    {
        Destroy(shieldInstance);
        shieldInstance = null;
        playerMovement.isAttacking = false;
    }

    // Handles the UI Button press (simulates holding down the key)
    public void ShieldButtonDown()
    {
        isShieldButtonHeld = true;
    }

    // Handles the UI Button release (simulates key release)
    public void ShieldButtonUp()
    {
        isShieldButtonHeld = false;
    }

    void UpdateShieldPosition()
    {
        if (shieldInstance == null) return;

        // Position the shield in front of the player based on lastFacingDirection
        Vector2 shieldPosition = (Vector2)transform.position + playerMovement.lastFacingDirection * shieldOffset.magnitude;
        shieldInstance.transform.position = shieldPosition + new Vector2(0f, 0.5f); // --LCC

        // Rotate shield to face the direction the player is facing
        float angle = Mathf.Atan2(playerMovement.lastFacingDirection.y, playerMovement.lastFacingDirection.x) * Mathf.Rad2Deg;
        shieldInstance.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
