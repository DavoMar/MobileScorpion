using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSortingOrder : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Transform player;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Find the player by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player1");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Player not found! Make sure the player has the 'Player1' tag.");
        }
    }

    void Update()
    {
        if (player == null) return;

        // Compare Y positions to determine sorting order
        if (player.position.y > transform.position.y)
        {
            spriteRenderer.sortingOrder = 10; // In front of the player
        }
        else
        {
            spriteRenderer.sortingOrder = 5; // Behind the player
        }
    }
}
