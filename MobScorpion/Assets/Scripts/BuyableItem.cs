using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyableItem : MonoBehaviour
{
    public int cost = 10; // The cost of the item
    public string itemName = "Item"; // Name of the item (can be set for clarity)

    // Effect when bought - this will be overridden by the specific item effect
    public virtual void ApplyEffect(PlayerBuy player)
    {
        // Default behavior: can be overridden for specific items
        Debug.Log(itemName + " applied to Player " + player.playerID);
    }
}