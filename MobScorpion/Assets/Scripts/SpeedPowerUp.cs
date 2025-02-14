using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : BuyableItem
{
    public float speedMultiplier = 1.5f; // How much to increase the player's speed

    
    public override void ApplyEffect(PlayerBuy player)
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>(); 
        if (playerMovement != null)
        {
            playerMovement.ApplyPermanentSpeedBoost(speedMultiplier); 
            Debug.Log("Player " + player.playerID + " speed increased by a factor of " + speedMultiplier);

           
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("Player does not have a PlayerMovement component.");
        }
    }
}
