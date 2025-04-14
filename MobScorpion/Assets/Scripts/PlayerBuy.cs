using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBuy : MonoBehaviour
{
    public int playerID; // Unique ID to differentiate players
    public float buyRange = 2f; // Maximum distance to allow purchase
    public GameObject buyButtonPrefab; // UI button prefab
    public GameObject[] canvasesToDisable; // Array of canvases to disable
    public Sprite buyableSprite; // Green sprite for buyable state
    public Sprite notBuyableSprite; // Red sprite for not buyable state

    public CoinManager coinManager;
    private GameObject buyButton;
    private BuyableItem nearbyItem;
    private Image buttonImage;

    void Start()
    {
        coinManager = FindObjectOfType<CoinManager>();
        
        // Instantiate the button but keep it hidden initially
        if (buyButtonPrefab)
        {
            buyButton = Instantiate(buyButtonPrefab, FindObjectOfType<Canvas>().transform);
            buyButton.GetComponent<Button>().onClick.AddListener(TryToBuyItem);
            buttonImage = buyButton.GetComponent<Image>();
            buyButton.SetActive(false);
        }
    }

    void Update()
    {
        DetectNearbyItem();
    }

    private void DetectNearbyItem()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, buyRange);
        nearbyItem = null;
        
        foreach (Collider2D collider in colliders)
        {
            BuyableItem item = collider.GetComponent<BuyableItem>();
            if (item != null)
            {
                nearbyItem = item;
                break;
            }
        }
        
        // Show or hide the buy button based on proximity to an item and update sprite
        if (buyButton)
        {
            bool buttonActive = nearbyItem != null;
            buyButton.SetActive(buttonActive);
            UpdateButtonSprite();
            ToggleCanvases(!buttonActive);
        }
    }

    private void UpdateButtonSprite()
    {
        if (nearbyItem == null || buttonImage == null) return;
        
        int playerCoins = (playerID == 1) ? coinManager.player1Coins : coinManager.player2Coins;
        
        if (playerCoins >= nearbyItem.cost)
        {
            buttonImage.sprite = buyableSprite; // Set to green sprite if affordable
        }
        else
        {
            buttonImage.sprite = notBuyableSprite; // Set to red sprite if not affordable
        }
    }

    private void ToggleCanvases(bool state)
    {
        foreach (GameObject canvas in canvasesToDisable)
        {
            if (canvas != null)
            {
                canvas.SetActive(state);
            }
        }
    }

    public void TryToBuyItem()
    {
        if (nearbyItem == null) return;
        
        int playerCoins = (playerID == 1) ? coinManager.player1Coins : coinManager.player2Coins;

        if (playerCoins >= nearbyItem.cost)
        {
            coinManager.AddCoins(playerID, -nearbyItem.cost);
            nearbyItem.ApplyEffect(this);
            Debug.Log("Player " + playerID + " bought an item!");
            Destroy(nearbyItem.gameObject);
            
            // Hide the button after purchase
            if (buyButton)
            {
                buyButton.SetActive(false);
            }
            
            // Re-enable the canvases after purchase
            ToggleCanvases(true);
        }
        else
        {
            Debug.Log("Not enough coins to buy this item.");
        }
    }
}