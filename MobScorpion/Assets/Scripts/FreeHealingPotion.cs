using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeHealingPotion : MonoBehaviour
{
    public int healingAmount = 5; // Amount of health to restore when the potion is used

    // When the player enters the trigger area, apply healing and destroy the potion
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object has a Health component (i.e., the player)
        Health playerHealth = other.GetComponent<Health>();

        if (playerHealth != null)
        {
            // Restore health to the player
            playerHealth.HealDamage(healingAmount);

            // Log the healing effect for debugging
            Debug.Log("Player healed by " + healingAmount + " points.");

            // Destroy the healing potion object after it is used
            Destroy(gameObject);
        }
    }
}
