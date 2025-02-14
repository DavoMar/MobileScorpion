using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// spawns a sword hitbox when a key is pressed, tells it how much damage to do, and then deletes after a duration
public class SwordSwing : MonoBehaviour
{
    public GameObject hitboxPrefab;
    public KeyCode swingButton = KeyCode.Z;
    public float swingDuration = 0.5f;
    public int damageAmount = 5;
    public Vector2 hitboxOffset = new Vector2(1f, 0f);

    private PlayerMovement playerMovement;
    private bool isSwinging = false;
    public int playerID; // Added playerID
    public GameObject swordSpritePrefab; // Visual-only sword sprite prefab
    public Vector2 swordSpriteOffset = new Vector2(0.3f, 1f);
    private GameObject swordSpriteInstance; 

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>(); // Get the PlayerMovement script
        playerID = playerMovement.playerID; // Set playerID from PlayerMovement
    }

    void Update()
    {
        if (Input.GetKeyDown(swingButton) && !isSwinging && !playerMovement.isAttacking)
        {
            StartCoroutine(SwingSword());
        }
    }

IEnumerator SwingSword()
{
    isSwinging = true;
    playerMovement.isAttacking = true;

    // Disable player movement during the swing
    playerMovement.enabled = false;

    // Calculate the default spawn position
    Vector2 spawnPosition = (Vector2)transform.position + playerMovement.lastFacingDirection * hitboxOffset.magnitude + new Vector2(0f, 0.25f);
    GameObject hitbox = Instantiate(hitboxPrefab, spawnPosition, Quaternion.identity, transform);

    // Calculate rotation for the hitbox based on direction
    float angle = Mathf.Atan2(playerMovement.lastFacingDirection.y, playerMovement.lastFacingDirection.x) * Mathf.Rad2Deg;
    hitbox.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

    // Calculate the default sword position based on lastFacingDirection
    Vector2 swordPosition = (Vector2)transform.position + playerMovement.lastFacingDirection * swordSpriteOffset;

    // Set a tolerance value for diagonal direction checks
    float tolerance = 0.1f;

    // Adjust the sword's position based on the facing direction
    if (Mathf.Approximately(playerMovement.lastFacingDirection.x, 0f) && playerMovement.lastFacingDirection.y < 0)
    {
        swordPosition += new Vector2(-0.2f, 0.75f); // Adjust for facing down
    }
    else if (Mathf.Approximately(playerMovement.lastFacingDirection.y, 0f) && playerMovement.lastFacingDirection.x > 0)
    {
        swordPosition += new Vector2(0.1f, 0.25f); // Adjust for facing right
    }
    else if (playerMovement.lastFacingDirection.x > tolerance && playerMovement.lastFacingDirection.y < -tolerance) // Down-right
    {
        swordPosition += new Vector2(0.3f, 0.75f); // Adjust for facing down-right
    }
    else if (playerMovement.lastFacingDirection.x < -tolerance && playerMovement.lastFacingDirection.y < -tolerance) // Down-left
    {
        swordPosition += new Vector2(0.25f, 0.55f); // Adjust for facing down-left
    }

    // Instantiate the sword sprite with the adjusted position
    swordSpriteInstance = Instantiate(swordSpritePrefab, swordPosition, Quaternion.identity, transform);
    swordSpriteInstance.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + -30f));

    // Apply damage to any enemies in the hitbox
    SwordHitbox swordHitbox = hitbox.GetComponent<SwordHitbox>();
    if (swordHitbox != null)
    {
        swordHitbox.DamageAmount = damageAmount;
        swordHitbox.playerID = playerID; // Pass playerID to hitbox
    }

    Collider2D collider = hitbox.GetComponent<Collider2D>();
    if (collider != null)
    {
        collider.isTrigger = false; // Ensure it's set for collision, not trigger
    }

    // Wait for the swing duration
    yield return new WaitForSeconds(swingDuration);

    // Re-enable player movement after the swing
    playerMovement.enabled = true;

    // Destroy the hitbox and sword sprite
    Destroy(hitbox);
    Destroy(swordSpriteInstance);

    isSwinging = false;
    playerMovement.isAttacking = false;
}


public void IncreaseDamage(int amount)
    {
        damageAmount += amount; // Increase the damage by the specified amount
    }
}
