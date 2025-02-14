using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBridge : MonoBehaviour
{
    public Sprite closedSprite;
    //public Sprite closedSpritetwo;
    public Sprite openSprite;
    //public Sprite openSpritetwo;
    private SpriteRenderer spriteRenderer;
    private bool isOpen = false;
    public Pressureplate pressurePlate;
    public BoxCollider2D doorCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = closedSprite;
        doorCollider = GetComponent<BoxCollider2D>();
        //doorCollider.isTrigger = true;
    }

    void Update()
    {
        if (pressurePlate != null && pressurePlate.isitActive())
        {
            if (!isOpen)
            {
                isOpen = true;
                spriteRenderer.sprite = openSprite;
                doorCollider.enabled = true; // Disable the collider to allow passage
                //doorCollider.isTrigger = false;
            }
        }
        else
        {
            if (isOpen)
            {
                isOpen = false;
                spriteRenderer.sprite = closedSprite;
                //doorCollider.isTrigger = true;
                doorCollider.enabled = false; // Enable the collider to block passage
            }
        }
    }
}
