using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float arcHeight = 2f;
    public int coinValue = 5; // Value of the coin when collected

    private int playerID;

    public void Initialize(int id)
    {
        playerID = id;
        StartCoroutine(MoveToPlayer());
    }

    private IEnumerator MoveToPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player" + playerID);
        if (player == null)
        {
            yield break;
        }

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = player.transform.position;

        float duration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            float height = arcHeight * Mathf.Sin(t * Mathf.PI);
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, t);
            newPosition.y += height;

            transform.position = newPosition;
            yield return null;
        }

        // After reaching the player, add the coins to the player's coin count
        CoinManager coinManager = FindObjectOfType<CoinManager>(); // Find the CoinManager
        if (coinManager != null)
        {
            coinManager.AddCoins(playerID, coinValue); // Add coins to the correct player
        }

        // Destroy the coin after it is collected
        Destroy(gameObject);
    }
}
