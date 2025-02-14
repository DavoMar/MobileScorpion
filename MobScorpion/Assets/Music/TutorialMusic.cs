using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TutorialMusic : MonoBehaviour
{

    [SerializeField] private Camera cam;
    [SerializeField] private bool playOnCameraMove; // if false, the song will play when Start() is called
    [SerializeField] private float fadeTime; // how long it should take for the music to fade, in seconds

    private AudioSource song;
    private float timeOfTrigger = float.NegativeInfinity;
    private float camStartingY;

    void Start ()
    {
        song = GetComponent<AudioSource> ();
        if (!playOnCameraMove)
        {
            song.Play ();
        }
        camStartingY = cam.transform.position.y;
    }

    void Update ()
    {
        // decrease the volume over time, once triggered
        if (song.volume > 0f && timeOfTrigger + fadeTime > Time.time)
        {
            song.volume -= Time.deltaTime / fadeTime;
        }
        if (!song.isPlaying && playOnCameraMove && cam.transform.position.y > camStartingY)
        {
            song.Play ();
        }
    }

    void OnTriggerEnter2D (Collider2D c)
    {
        if (timeOfTrigger == float.NegativeInfinity)
            timeOfTrigger = Time.time;
    }
}
