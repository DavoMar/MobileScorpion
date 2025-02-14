using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public GameObject newPlayerPrefab;       // The new player prefab (with the different sprite)
    public Image fadeImage;                  // UI Image component for screen fading
    public float fadeDuration = 2f;          // Duration of the fade to black effect
    public string nextSceneName;             // Name of the scene to load after fade
    public AudioSource currentMusic;         // The current music AudioSource to stop
    public AudioClip newMusic;               // The new music to play upon item pickup

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<SpriteRenderer>() != null)
        {
            // Get the position of the player object
            Vector3 playerPosition = other.transform.position;

            // Destroy the current player object
            Destroy(other.gameObject);

            // Instantiate the new player object at the same position
            Instantiate(newPlayerPrefab, playerPosition, Quaternion.identity);

            // Make the item invisible
            MakeInvisible();

            // Start the fade and scene transition sequence
            StartCoroutine(EndSequence());
        }
    }

    private void MakeInvisible()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false; // Disable the SpriteRenderer to make it invisible
        }
    }

    private IEnumerator EndSequence()
    {
        // Step 1: Stop current music if it's playing
        if (currentMusic != null && currentMusic.isPlaying)
        {
            currentMusic.Stop();
        }

        // Step 2: Play the new music upon pickup
        if (newMusic != null)
        {
            currentMusic.PlayOneShot(newMusic); // Play the new music
        }

        // Step 3: Screen fade to black effect
        float elapsedTime = 0f;
        Color fadeColor = fadeImage.color;
        while (elapsedTime < fadeDuration)
        {
            fadeColor.a = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            fadeImage.color = fadeColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fadeColor.a = 1;
        fadeImage.color = fadeColor;

        // Step 4: Linger for 5 seconds after fully black
        yield return new WaitForSeconds(5f);

        // Step 5: Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }
}
