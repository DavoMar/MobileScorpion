using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pressureplate : MonoBehaviour
{
    private bool inContactP1 = false;
    private bool inContactP2 = false;
    public bool playOnePlate = false;
    public bool playTwoPlate = false;
    private bool isActive = false;
    private Renderer plateRenderer;
    private SpriteRenderer spriteRenderer;
    public Color activeColor = Color.blue;
    private Color originalColor;
    public Sprite openSprite;
    private Sprite closedSprite;
    public float activeDuration = 2f; // Time the plate remains active after the player steps off
    private Coroutine deactivateCoroutine;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        closedSprite = spriteRenderer.sprite;
        plateRenderer = GetComponent<Renderer>();
        originalColor = plateRenderer.material.color;
    }

    void Update()
    {
        if ((inContactP1 && playOnePlate) || (inContactP2 && playTwoPlate))
        {
            ActivatePlate();
            if (deactivateCoroutine != null)
            {
                StopCoroutine(deactivateCoroutine);
            }
        }
    }

    public bool isitActive()
    {
        return isActive;
    }

    private void setPlayer(int x)
    {
        if (x == 1)
        {
            playOnePlate = true;
        }
        if (x == 2)
        {
            playTwoPlate = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            inContactP1 = true;
        }

        if (collision.gameObject.CompareTag("Player2"))
        {
            inContactP2 = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            inContactP1 = false;
            if (playOnePlate)
            {
                StartDeactivationTimer();
            }
        }

        if (collision.gameObject.CompareTag("Player2"))
        {
            inContactP2 = false;
            if (playTwoPlate)
            {
                StartDeactivationTimer();
            }
        }
    }

    private void ActivatePlate()
    {
        plateRenderer.material.color = activeColor;
        spriteRenderer.sprite = openSprite;
        isActive = true;
    }

    private void StartDeactivationTimer()
    {
        if (deactivateCoroutine != null)
        {
            StopCoroutine(deactivateCoroutine);
        }
        deactivateCoroutine = StartCoroutine(DeactivatePlateAfterDelay());
    }

    private IEnumerator DeactivatePlateAfterDelay()
    {
        yield return new WaitForSeconds(activeDuration);
        isActive = false;
        plateRenderer.material.color = originalColor;
        spriteRenderer.sprite = closedSprite;
    }
}
