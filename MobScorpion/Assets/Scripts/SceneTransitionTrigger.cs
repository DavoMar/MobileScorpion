using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionTrigger : MonoBehaviour
{
    public Image fadeImage;              // Assign your full-screen black Image here
    public float fadeDuration = 0.7f;      // Duration of the fade effect
    public string sceneToLoad;           // Name of the scene to load

    private bool isFading = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Start the fade and scene transition when the player enters the trigger
        if (other.CompareTag("Player1") && !isFading)
        {
            StartCoroutine(FadeAndSwitchScene());
        }
    }

    private IEnumerator FadeAndSwitchScene()
    {
        isFading = true;

        // Fade to black
        float timeElapsed = 0f;
        Color color = fadeImage.color;
        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(timeElapsed / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        // Load the new scene
        SceneManager.LoadScene(sceneToLoad);
    }
}