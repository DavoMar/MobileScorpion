using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Sprite closedSprite;
    public Sprite openSprite;
    private SpriteRenderer spriteRenderer;
    private bool isOpen = false;
    public Pressureplate pressurePlate;
    public BoxCollider2D doorCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //spriteRenderer.sprite = closedSprite;
        doorCollider = GetComponent<BoxCollider2D>();
        doorCollider.isTrigger = false; // Set the collider to trigger mode
    }

    void Update()
    {
        if (pressurePlate != null && pressurePlate.isitActive())
        {
            if (!isOpen)
            {
                isOpen = true;
                spriteRenderer.sprite = openSprite;
                doorCollider.isTrigger = true;
                doorCollider.enabled = false; // Disable the collider to allow passage
            }
        }
        else
        {
            if (isOpen)
            {
                isOpen = false;
                spriteRenderer.sprite = closedSprite;
                doorCollider.isTrigger = false;
                doorCollider.enabled = true; // Enable the collider to block passage
            }
        }
    }
}
