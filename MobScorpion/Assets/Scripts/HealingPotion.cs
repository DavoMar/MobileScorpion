using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPotion : BuyableItem
{
    public int healingAmount = 5; // Amount of health to restore when the potion is used


    public override void ApplyEffect(PlayerBuy player)
    {
        Health playerHealth = player.GetComponent<Health>(); 
        if (playerHealth != null)
        {
            playerHealth.HealDamage(healingAmount); 
            Debug.Log("Player " + player.playerID + " healed by " + healingAmount + " health with " + itemName);

            // Destroy the potion item after it is used
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("Player does not have a Health component.");
        }
    }
}
