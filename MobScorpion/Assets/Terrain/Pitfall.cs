using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitfall : MonoBehaviour
{

    [SerializeField] private List<Vector2> tileLoc; // the locations of the tiles to destroy (use the lower lefthand corner of the tiles)
    [SerializeField] private FloorDestroyer floorDestroyer;
    [SerializeField] private Camera cam;

    private float minDestroyRadius; // min & max distance at which the tiles will fall
    private float maxDestroyRadius;
    private float destroyRadius;

    void Start()
    {
        maxDestroyRadius = -cam.transform.position.z / 5f * 6f; // min & max destroy radii depend on the camera's z pos
        minDestroyRadius = maxDestroyRadius - 4;
        destroyRadius = Random.Range (minDestroyRadius, maxDestroyRadius);
    }

    void Update()
    {
        if (transform.position.y - destroyRadius < cam.transform.position.y)
        {
            floorDestroyer.DestroyTiles (tileLoc);
            Destroy (gameObject);
        }
    }
}
