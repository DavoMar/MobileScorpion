using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredPitfall : MonoBehaviour
{

    [SerializeField] private List<Vector2> tileLoc; // the locations of the tiles to destroy (use the lower lefthand corner of the tiles)
    [SerializeField] private FloorDestroyer floorDestroyer;
    public Pressureplate pressurePlate;

    void Update()
    {
        if (pressurePlate != null && pressurePlate.isitActive ())
        {
            floorDestroyer.DestroyTiles (tileLoc);
            Destroy (gameObject);
        }
    }
}
