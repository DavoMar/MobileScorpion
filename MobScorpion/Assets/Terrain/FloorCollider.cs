using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCollider : MonoBehaviour
{
    void OnTriggerExit2D (Collider2D other)
    {
        Health otherHealth = other.GetComponent<Health> ();
        if (otherHealth)
        {
            otherHealth.Die ();
        }
    } 
}
