using System.Collections;
using System.Collections.Generic; // Needed for List<>
using UnityEngine;
using UnityEngine.Rendering.Universal; // Required for Light2D
using UnityEngine.SceneManagement;

public class SceneTransitionTrigger : MonoBehaviour
{
    public GameObject[] objectsWithLights; // Objects that contain Light2D components
    public GameObject[] objectsToDisable;  // Objects that get disabled/enabled during transition
    public float fadeDuration = 0.7f;      // Duration of the fade effect
    public float transitionTime = 1.5f;    // Total time before switching scenes (should be >= fadeDuration)
    public string sceneToLoad;             // Name of the scene to load
    public bool fadeInInstead = false;     // If true, fade in instead of fading out

    private bool isTransitioning = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1") && !isTransitioning)
        {
            StartCoroutine(FadeLightsAndSwitchScene());
        }
    }

    private IEnumerator FadeLightsAndSwitchScene()
    {
        isTransitioning = true;

        // Always disable objects at the start to ensure a clean transition
        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        // Get all Light2D components from assigned objects
        Light2D[] lights = GetAllLight2DComponents();

        // Store the initial intensity of each light
        float[] initialIntensities = new float[lights.Length];
        for (int i = 0; i < lights.Length; i++)
        {
            initialIntensities[i] = lights[i].intensity;
        }

        // Ensure fadeDuration does not exceed transitionTime
        fadeDuration = Mathf.Min(fadeDuration, transitionTime);

        float timeElapsed = 0f;

        if (fadeInInstead)
        {
            // Set all lights to 0 at the start of the fade-in
            foreach (var light in lights)
            {
                if (light != null)
                    light.intensity = 0f;
            }

            // Gradually increase light intensity
            while (timeElapsed < fadeDuration)
            {
                timeElapsed += Time.deltaTime;
                float fadeFactor = timeElapsed / fadeDuration; // 0 to 1

                for (int i = 0; i < lights.Length; i++)
                {
                    if (lights[i] != null)
                    {
                        lights[i].intensity = initialIntensities[i] * fadeFactor;
                    }
                }
                yield return null;
            }

            // Ensure lights reach full intensity
            for (int i = 0; i < lights.Length; i++)
            {
                if (lights[i] != null)
                {
                    lights[i].intensity = initialIntensities[i];
                }
            }

            // Enable objects after fade-in is complete
            foreach (GameObject obj in objectsToDisable)
            {
                if (obj != null)
                    obj.SetActive(true);
            }
        }
        else
        {
            // Gradually decrease light intensity (fade-out)
            while (timeElapsed < fadeDuration)
            {
                timeElapsed += Time.deltaTime;
                float fadeFactor = 1 - (timeElapsed / fadeDuration); // 1 to 0

                for (int i = 0; i < lights.Length; i++)
                {
                    if (lights[i] != null)
                    {
                        lights[i].intensity = initialIntensities[i] * fadeFactor;
                    }
                }
                yield return null;
            }

            // Ensure lights are completely off
            foreach (var light in lights)
            {
                if (light != null)
                    light.intensity = 0f;
            }

            // Wait the remaining time before loading the next scene
            yield return new WaitForSeconds(transitionTime - fadeDuration);

            // Load the new scene
            SceneManager.LoadScene(sceneToLoad);
        }

        isTransitioning = false;
    }

    // Helper function to get Light2D components from assigned objects
    private Light2D[] GetAllLight2DComponents()
    {
        List<Light2D> lights = new List<Light2D>();
        foreach (GameObject obj in objectsWithLights)
        {
            if (obj != null)
            {
                Light2D light = obj.GetComponent<Light2D>();
                if (light != null)
                {
                    lights.Add(light);
                }
            }
        }
        return lights.ToArray();
    }
}
