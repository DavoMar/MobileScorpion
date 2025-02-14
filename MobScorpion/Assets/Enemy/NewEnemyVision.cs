using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyVision : MonoBehaviour
{
    public List<GameObject> objects = new List<GameObject> ();

    void OnTriggerEnter2D (Collider2D other)
    {
        objects.Add (other.gameObject);
    }

    void OnTriggerExit2D (Collider2D other)
    {
        objects.Remove (other.gameObject);
    }
}
