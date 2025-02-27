using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles arrow shooting mechanics
public class ArrowShooter : MonoBehaviour
{
    public GameObject arrowPrefab; // Reference to the arrow prefab
    public KeyCode shootButton = KeyCode.Space; // Button to shoot
    public float shootCooldown = 0.5f; // Time between shots
    private float lastShootTime = 0f; // Last time the player shot an arrow
    private bool isShooting = false;
    private bool isShootButtonHeld = false; // Track UI button press

    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // Check for shooting input and cooldown
        if (isShooting && Time.time > lastShootTime + shootCooldown)
        {
            isShooting = false;
            playerMovement.isAttacking = false;
        }

        // Check if either the keyboard or the UI button is pressed
        if ((Input.GetKey(shootButton) || isShootButtonHeld) && !isShooting && !playerMovement.isAttacking)
        {
            ShootArrow();
            lastShootTime = Time.time;
            playerMovement.isAttacking = true;
            isShooting = true;
        }
    }

    void ShootArrow()
    {
        // Get the current shooting direction from the player movement
        Vector2 shootingDirection = playerMovement.lastFacingDirection;

        // Instantiate the arrow prefab at the player's position with an offset
        GameObject arrow = Instantiate(arrowPrefab, (Vector2)transform.position + shootingDirection * 0.5f + Vector2.up * 0.25f, Quaternion.identity);

        // Get the Arrow component and set its direction
        Arrow arrowComponent = arrow.GetComponent<Arrow>();
        arrowComponent.SetDirection(shootingDirection);

        // Rotate the arrow to match the shooting direction
        float angle = Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Set the damage of the arrow based on player upgrades
        arrowComponent.IncreaseDamage(playerMovement.GetArrowDamage());

        // Assign the player ID to the arrow
        arrowComponent.playerID = playerMovement.playerID;
    }

    // UI Button Functions
    public void ShootButtonDown()
    {
        isShootButtonHeld = true;
    }

    public void ShootButtonUp()
    {
        isShootButtonHeld = false;
    }
}
