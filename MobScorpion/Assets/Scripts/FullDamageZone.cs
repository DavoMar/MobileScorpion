using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullDamageZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Set the player to full damage mode upon entering
            other.GetComponent<PlayerCombat>().isInFullDamageZone = true;
            Debug.Log(other.name + " entered full damage zone.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Revert to minimal damage mode upon exiting
            other.GetComponent<PlayerCombat>().isInFullDamageZone = false;
            Debug.Log(other.name + " exited full damage zone.");
        }
    }
}
