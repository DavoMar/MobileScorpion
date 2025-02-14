using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    // Damage values for each type of attack
    public int minimalArrowDamage = 2;
    public int fullArrowDamage = 5;
    public int minimalSwordDamage = 5;
    public int fullSwordDamage = 10;

    public bool isInFullDamageZone = false; // Tracks if the player is in the full damage zone

    // List of intended target tags
    public List<string> intendedTargets = new List<string> { "Enemy", "Player" }; // Add target tags as needed

    // Method to deal damage to a target with specified attack type
    public void DealDamage(Health target, string attackType)
    {
        int damageAmount = 0;

        // Determine damage based on attack type and zone
        if (attackType == "Arrow")
        {
            damageAmount = isInFullDamageZone ? fullArrowDamage : minimalArrowDamage;
        }
        else if (attackType == "Sword")
        {
            damageAmount = isInFullDamageZone ? fullSwordDamage : minimalSwordDamage;
        }

        // Apply the appropriate damage to the target, with half damage if it's not the intended target
        bool survived = target.TakeDamage(damageAmount, intendedTargets);

        // Log feedback based on survival status
        if (!survived)
        {
            Debug.Log(target.name + " was destroyed by " + attackType + " dealing " + damageAmount + " damage.");
        }
        else
        {
            Debug.Log(target.name + " survived with " + target.GetHealth() + " health after taking " + damageAmount + " damage from " + attackType);
        }
    }
}