using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCollider : MonoBehaviour
{

    public int numPlayersInShop;

    void OnTriggerEnter2D (Collider2D c)
    {
        if (c.tag == "Player1" || c.tag == "Player2")
            ++numPlayersInShop;
    }

    void OnTriggerExit2D (Collider2D c)
    {
        if (c.tag == "Player2" || c.tag == "Player1")
            --numPlayersInShop;
    }
}
