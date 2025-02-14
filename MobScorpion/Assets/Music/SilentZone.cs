using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilentZone : MonoBehaviour
{
    public int numPlayersInSilentZone;
    public Sprite closedDoorSprite; // Sprite for the closed door
    public Sprite openDoorSprite;   // Sprite for the open door
    public GameObject door;         // Reference to the door GameObject
    private bool isDoorOpen = false;

    void Start()
    {
        // Set the door to its closed sprite initially
        SetDoorSprite(closedDoorSprite);
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if ((c.tag == "Player1" || c.tag == "Player2"))
        {
            ++numPlayersInSilentZone;
            CheckDoorState();
        }
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if ((c.tag == "Player1" || c.tag == "Player2"))
        {
            --numPlayersInSilentZone;
            CheckDoorState();
        }
    }

    void CheckDoorState()
    {
        if (numPlayersInSilentZone == 2 && isDoorOpen)
        {
            CloseDoor();
        }
        else if (numPlayersInSilentZone == 1 && !isDoorOpen)
        {
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        SetDoorSprite(openDoorSprite);
        isDoorOpen = true;

        // Disable the collider on the door
        if (door != null)
        {
            Collider2D doorCollider = door.GetComponent<Collider2D>();
            if (doorCollider != null)
            {
                doorCollider.enabled = false;
            }
        }
    }

    void CloseDoor()
    {
        SetDoorSprite(closedDoorSprite);
        isDoorOpen = false;

        // Enable the collider on the door
        if (door != null)
        {
            Collider2D doorCollider = door.GetComponent<Collider2D>();
            if (doorCollider != null)
            {
                doorCollider.enabled = true;
            }
        }
    }

    void SetDoorSprite(Sprite newSprite)
    {
        if (door != null)
        {
            SpriteRenderer doorSpriteRenderer = door.GetComponent<SpriteRenderer>();
            if (doorSpriteRenderer != null)
            {
                doorSpriteRenderer.sprite = newSprite;
            }
        }
    }
}
