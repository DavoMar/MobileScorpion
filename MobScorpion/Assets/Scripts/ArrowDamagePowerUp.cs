using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDamagePowerUp : BuyableItem
{
    public int damageIncreaseAmount = 1; // The amount of arrow damage to increase

    // Override the ApplyEffect method from BuyableItem
    public override void ApplyEffect(PlayerBuy player)
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>(); // Get the player's PlayerMovement component
        if (playerMovement != null)
        {
            playerMovement.IncreaseArrowDamage(damageIncreaseAmount); // Apply the arrow damage increase
            Debug.Log("Player " + player.playerID + " increased arrow damage by " + damageIncreaseAmount);

            // Destroy the power-up item after it is used
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("Player does not have a PlayerMovement component.");
        }
    }
}