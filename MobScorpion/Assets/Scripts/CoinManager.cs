using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public int player1Coins = 0;  // Coin count for player 1
    public int player2Coins = 0;  // Coin count for player 2

    public UICoinManager uiCoinManager1;
    public UICoinManager uiCoinManager2;

    // Method to increase coin count for a player
    public void AddCoins(int playerID, int amount)
    {
        if (playerID == 1)
        {
            player1Coins += amount;
            Debug.Log("Player 1's coins: " + player1Coins);
            uiCoinManager1.SetCoins (player1Coins); // update ui
        }
        else if (playerID == 2)
        {
            player2Coins += amount;
            Debug.Log("Player 2's coins: " + player2Coins);
            uiCoinManager2.SetCoins (player2Coins);
        }
    }

    // Method to subtract coins for a player (used when purchasing items)
    public bool SpendCoins(int playerID, int amount)
    {
        if (playerID == 1 && player1Coins >= amount)
        {
            player1Coins -= amount;
            Debug.Log("Player 1 spent " + amount + " coins. Remaining: " + player1Coins);
            uiCoinManager1.SetCoins (player1Coins);
            return true;
        }
        else if (playerID == 2 && player2Coins >= amount)
        {
            player2Coins -= amount;
            Debug.Log("Player 2 spent " + amount + " coins. Remaining: " + player2Coins);
            uiCoinManager2.SetCoins (player2Coins);
            return true;
        }
        else
        {
            Debug.Log("Not enough coins for player " + playerID);
            return false;
        }
    }
}
