using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamagePowerUp : BuyableItem
{
    public int damageIncreaseAmount = 5; 
   
    public override void ApplyEffect(PlayerBuy player)
    {
        SwordSwing swordSwing = player.GetComponentInChildren<SwordSwing>(); 
        if (swordSwing != null)
        {
            swordSwing.IncreaseDamage(damageIncreaseAmount); // Increase the sword's damage
            Debug.Log("Player " + player.playerID + " increased sword damage by " + damageIncreaseAmount);

            
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("Player does not have a SwordSwing component.");
        }
    }
}
