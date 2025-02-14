using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuy : MonoBehaviour
{
    public int playerID; // Unique ID to differentiate players (e.g., 1 for Player 1, 2 for Player 2)
    public KeyCode buyKey = KeyCode.E; // The key to press to buy an item
    public float buyRange = 2f; // Maximum distance from the item to allow purchase

    private CoinManager coinManager;

    void Start()
    {
        coinManager = FindObjectOfType<CoinManager>(); // Reference to the CoinManager
    }

    void Update()
    {
        // Check if the player presses the buy key
        if (Input.GetKeyDown(buyKey))
        {
            TryToBuyItem();
        }
    }

    private void TryToBuyItem()
    {
        // Find all nearby items with a collider in a specified range
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, buyRange);

        foreach (Collider2D collider in colliders)
        {
            // Check if the item has a BuyableItem component
            BuyableItem item = collider.GetComponent<BuyableItem>();
            if (item != null)
            {
                int playerCoins = (playerID == 1) ? coinManager.player1Coins : coinManager.player2Coins;

                // Check if the player has enough coins to buy the item
                if (playerCoins >= item.cost)
                {
                    // Deduct the item cost and apply the item's effect
                    coinManager.AddCoins(playerID, -item.cost);
                    item.ApplyEffect(this); // Pass the player as a parameter to apply the item's effect
                    Debug.Log("Player " + playerID + " bought an item!");

                    // Optionally, destroy the item if it's a one-time purchase
                    Destroy(item.gameObject);
                    break; // Stop after purchasing one item
                }
                else
                {
                    Debug.Log("Not enough coins to buy this item.");
                }
            }
        }
    }
}
