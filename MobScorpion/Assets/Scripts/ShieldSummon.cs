using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// spawns a shield hitbox when key is pressed, keeps it facing the right way, and then deletes it when that key is lifted
public class ShieldSummon : MonoBehaviour
{
    public GameObject shieldPrefab; // Reference to the shield prefab
    public KeyCode shieldButton = KeyCode.LeftShift; // Button to activate the shield
    public Vector2 shieldOffset = new Vector2(0.5f, 0f); // Offset to position shield in front of player

    private GameObject shieldInstance; // Instance of the shield object
    private PlayerMovement playerMovement; // Reference to player movement to get direction

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>(); // Get the player movement component
    }

    void Update()
    {
        // Check if the shield button is being held down
        if (Input.GetKey(shieldButton))
        {
            if (shieldInstance == null && !playerMovement.isAttacking)
            {
                // Instantiate the shield if it's not already active
                shieldInstance = Instantiate(shieldPrefab, transform.position + new Vector3 (0f, 0.5f, 0f), Quaternion.identity); // added offset of 0.5 in the y direction --LCC
                shieldInstance.transform.parent = transform; // Make the shield follow the player
                shieldInstance.GetComponent<Shield> ().wielder = gameObject;
                playerMovement.isAttacking = true;
            }

            // Update shield position based on player's facing direction
            if (shieldInstance != null)
                UpdateShieldPosition();
        }
        else
        {
            // Destroy the shield if the button is released
            if (shieldInstance != null)
            {
                Destroy(shieldInstance);
                playerMovement.isAttacking = false;
            }
        }
    }

    void UpdateShieldPosition()
    {
        // Position the shield in front of the player based on lastFacingDirection
        Vector2 shieldPosition = (Vector2)transform.position + playerMovement.lastFacingDirection * shieldOffset.magnitude;
        shieldInstance.transform.position = shieldPosition + new Vector2 (0f, 0.5f); // --LCC

        // Rotate shield to face the direction the player is facing
        float angle = Mathf.Atan2(playerMovement.lastFacingDirection.y, playerMovement.lastFacingDirection.x) * Mathf.Rad2Deg;
        shieldInstance.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
    
}