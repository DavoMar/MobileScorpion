using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInObjects : MonoBehaviour
{
    public GameObject[] objectsToFade; // Assign objects to fade (Sprites or UI Images)
    public float fadeSpeed = 1f; // Speed of fade-in
    public float maxOpacity = 1f; // Target opacity

    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    private List<Image> uiImages = new List<Image>();

    private void Start()
    {
        // Separate objects into UI images and sprite renderers
        foreach (GameObject obj in objectsToFade)
        {
            if (obj != null)
            {
                SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                Image img = obj.GetComponent<Image>();

                if (sr != null)
                {
                    SetOpacity(sr, 0f);
                    spriteRenderers.Add(sr);
                }
                else if (img != null)
                {
                    SetOpacity(img, 0f);
                    uiImages.Add(img);
                }
            }
        }

        // Start fading in both sprite renderers and UI images
        StartCoroutine(FadeInSprites());
        StartCoroutine(FadeInUIImages());
    }

    private IEnumerator FadeInSprites()
    {
        bool allFaded = false;
        while (!allFaded)
        {
            allFaded = true;
            foreach (SpriteRenderer sr in spriteRenderers)
            {
                if (sr.color.a < maxOpacity)
                {
                    Color color = sr.color;
                    color.a += Time.deltaTime * fadeSpeed;
                    sr.color = color;
                    allFaded = false;
                }
            }
            yield return null;
        }

        // Ensure final opacity
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            SetOpacity(sr, maxOpacity);
        }
    }

    private IEnumerator FadeInUIImages()
    {
        bool allFaded = false;
        while (!allFaded)
        {
            allFaded = true;
            foreach (Image img in uiImages)
            {
                if (img.color.a < maxOpacity)
                {
                    Color color = img.color;
                    color.a += Time.deltaTime * fadeSpeed;
                    img.color = color;
                    allFaded = false;
                }
            }
            yield return null;
        }

        // Ensure final opacity
        foreach (Image img in uiImages)
        {
            SetOpacity(img, maxOpacity);
        }
    }

    private void SetOpacity(SpriteRenderer sr, float alpha)
    {
        Color color = sr.color;
        color.a = alpha;
        sr.color = color;
    }

    private void SetOpacity(Image img, float alpha)
    {
        Color color = img.color;
        color.a = alpha;
        img.color = color;
    }
}
