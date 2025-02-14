using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTile : MonoBehaviour
{

    private float timeOfSpawn;
    private Vector2 originalPosition;
    private float timeOfLastWobble = 0f;
    private Color currColor = new Color (1f, 1f, 1f, 1f);

    public float wobbleDuration = 1f; // in seconds 
    private float timeBetweenWobbles = 0.1f;
    private float fallDuration = 1f;
    private SpriteRenderer rend;

    void Start()
    {
        rend = GetComponent<SpriteRenderer> ();
        timeOfSpawn = Time.time;
        originalPosition = transform.position;
    }

    void Update()
    {
        if (timeOfSpawn + wobbleDuration > Time.time)
        {
            // wobble
            if (timeOfLastWobble + timeBetweenWobbles < Time.time)
            {
                transform.position = originalPosition + new Vector2 (Random.Range (-0.1f, 0.1f), Random.Range (-0.1f, 0.1f));
                timeOfLastWobble = Time.time;
            }
        }
        else
        {
            // fall
            if (timeOfLastWobble + fallDuration > Time.time)
            {
                if (rend.sortingOrder != -3) rend.sortingOrder= -3;
                transform.position += new Vector3 (0f, 0f, Time.deltaTime / fallDuration);
                currColor -= Color.white * Time.deltaTime / fallDuration;
                rend.color = currColor;
            }
            else
            {
                Destroy (gameObject);
            }
        }
    }
}
