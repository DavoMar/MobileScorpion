using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class BGMController : MonoBehaviour
{

    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioMixer sfxMixer;
    [SerializeField] private List<Pressureplate> doorButtons;
    [SerializeField] private List<ShopCollider> shopColliders;
    [SerializeField] private List<SilentZone> silentZones;
    [SerializeField] private AudioSource shopSong;
    [SerializeField] private List<AudioClip> shopSongClips;
    [SerializeField] private AudioSource finalFightSong;
    [SerializeField] private float transitionTime = 0.5f;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private Coroutine interpolationCoroutine;
    private bool doorIsOpen;
    private int numPlayersInShop;
    private int numPlayersInSilentZone;
    private int currSong = 0; // 1 means run, 2 means muffled run, 3 means shop, 4 means in silent zone
    private bool setup = true; // certain methods shouldn't happen during setup
    [SerializeField] private bool tutorial = false;

    void Start ()
    {
        if (PlayerPrefs.HasKey ("BGMVol"))
        {
            float bgmVol = PlayerPrefs.GetFloat ("BGMVol");
            mixer.SetFloat ("Master Volume", bgmVol);
            bgmSlider.value = bgmVol;
        }
        else
        {
            PlayerPrefs.SetFloat ("BGMVol", 0f);
        }
        if (PlayerPrefs.HasKey ("SFXVol"))
        {
            float sfxVol = PlayerPrefs.GetFloat ("SFXVol");
            sfxMixer.SetFloat ("Master Volume", sfxVol);
            sfxSlider.value = sfxVol;
        }
        else
        {
            PlayerPrefs.SetFloat ("SFXVol", 0f);
        }
        setup = false;
        if (tutorial)
        {
            mixer.SetFloat ("Run Volume", -80f);
            mixer.SetFloat ("Shop Volume", -80f);
        }
    }
    
    void Update ()
    {
        if (tutorial)
            return;

        // update variables
        doorIsOpen = false;
        foreach (Pressureplate p in doorButtons)
        {
            if (p.isitActive ()) doorIsOpen = true;
        }
        numPlayersInShop = 0;
        foreach (ShopCollider s in shopColliders)
        {
            numPlayersInShop += s.numPlayersInShop;
        }
        numPlayersInSilentZone = 0;
        foreach (SilentZone s in silentZones)
        {
            numPlayersInSilentZone += s.numPlayersInSilentZone;
        }

        // decide whether the current song should change
        if (numPlayersInSilentZone == 2)
        {
            // be silent, if not already
            if (currSong != 4)
            {
                shopSong.Stop ();
                currSong = 4;
                if (interpolationCoroutine != null)
                    StopCoroutine (interpolationCoroutine);
                interpolationCoroutine = StartCoroutine (InterpolateAudioEffect (transitionTime));
            }
        }
        else if (numPlayersInShop == 0)
        {
            // play run, if not already
            if (currSong != 1)
            {
                shopSong.Stop ();
                currSong = 1;
                if (interpolationCoroutine != null)
                    StopCoroutine (interpolationCoroutine);
                interpolationCoroutine = StartCoroutine (InterpolateAudioEffect (transitionTime));
            }
        }
        else if (numPlayersInShop > 0 && (numPlayersInShop < 2 || doorIsOpen))
        {
            // play muffled run, if not already
            if (currSong != 2)
            {
                shopSong.Stop ();
                currSong = 2;
                if (interpolationCoroutine != null)
                    StopCoroutine (interpolationCoroutine);
                interpolationCoroutine = StartCoroutine (InterpolateAudioEffect (transitionTime));
            }
        }
        else // numPlayersInShop == 2 && !doorIsOpen
        {
            // play shop, if not already
            if (currSong != 3)
            {
                shopSong.clip = shopSongClips[Random.Range (0, shopSongClips.Count)];
                shopSong.Play ();
                currSong = 3;
                if (interpolationCoroutine != null)
                    StopCoroutine (interpolationCoroutine);
                interpolationCoroutine = StartCoroutine (InterpolateAudioEffect (transitionTime));
            }
        }
    }

    private IEnumerator InterpolateAudioEffect (float duration)
    {
        // get current values of effects
        float runVol, lowpass, highpass, shopVol;
        mixer.GetFloat ("Run Volume", out runVol);
        mixer.GetFloat ("Run Lowpass", out lowpass);
        mixer.GetFloat ("Run Highpass", out highpass);
        mixer.GetFloat ("Shop Volume", out shopVol);

        // decide on target values
        float targetRunVol, targetLowpass, targetHighpass, targetShopVol;
        if (currSong == 1)
        {
            targetRunVol = -2f;
            targetLowpass = 22000f;
            targetHighpass = 10f;
            targetShopVol = -80f;
        }
        else if (currSong == 2)
        {
            targetRunVol = -7f;
            targetLowpass = 5000f;
            targetHighpass = 500f;
            targetShopVol = -80f;
        }
        else if (currSong == 3)
        {
            targetRunVol = -80f;
            targetLowpass = 5000f;
            targetHighpass = 500f;
            targetShopVol = -5f;
        }
        else // currSong == 4
        {
            targetRunVol = -80f;
            targetLowpass = 5000f;
            targetHighpass = 500f;
            targetShopVol = -80f;
        }

        // interpolate over time
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            // get next value for each effect
            float step = Mathf.SmoothStep (0f, 1f, elapsed / duration);
            float nextRunVol = Mathf.Lerp (runVol, targetRunVol, step);
            float nextLowpass = Mathf.Lerp  (lowpass, targetLowpass, step);
            float nextHighpass = Mathf.Lerp (highpass, targetHighpass, step);
            float nextShopVol = Mathf.Lerp (shopVol, targetShopVol, step);

            // set each effect to its next value
            mixer.SetFloat ("Run Volume", nextRunVol);
            mixer.SetFloat ("Run Lowpass", nextLowpass);
            mixer.SetFloat ("Run Highpass", nextHighpass);
            mixer.SetFloat ("Shop Volume", nextShopVol);

            yield return null;
        }

        // finally set everything to its target value
        mixer.SetFloat ("Run Volume", targetRunVol);
        mixer.SetFloat ("Run Lowpass", targetLowpass);
        mixer.SetFloat ("Run Highpass", targetHighpass);
        mixer.SetFloat ("Shop Volume", targetShopVol);
    }

    public void SetMusicVolume (float volume) 
    {
        if (setup)
            return;

        volume = volume == -30 ? -80 : volume;
        mixer.SetFloat ("Master Volume", volume);
        PlayerPrefs.SetFloat ("BGMVol", volume);
    }

    public void SetSFXVolume (float volume)
    {
        if (setup)
            return;

        volume = volume == -30 ? -80 : volume;
        sfxMixer.SetFloat ("Master Volume", volume);
        PlayerPrefs.SetFloat ("SFXVol", volume);
    }

    public void BeginFinalFight ()
    {
        finalFightSong.Play ();
    }

































    /*
    private bool doorIsOpen;
    private bool isInShop;
    private float currRunVolFactor = 1f;

    private float muffleSpeed = 1f;
    private float muffRunVolFactorGoal = 0.5f;

    void Update ()
    {
        // update variables
        doorIsOpen = false;
        foreach (Pressureplate p in doorButtons)
        {
            if (p.isItActive ()) doorIsOpen = true;
        }
        isInShop = false;
        foreach (ShopCollider s in shopColliders)
        {
            if (s.isItActive ()) isInShop = true;
        }
        float runVol, runHigh, runLow, shopVol;
        mixer.GetFloat ("Run Volume", out runVol);
        mixer.GetFloat ("Run Highpass", out runHigh);
        mixer.GetFloat ("Run Lowpass", out lowPass);
        mixer.GetFloat ("Shop Volume", out shopVol);

        // adjust music volume
        if (isInShop)
        {
            if (doorIsOpen)
            {
                // play muffled run music
                if (currRunVolFactor < muffRunVolFactorGoal - 0.01f || currRunVolFactor > muffRunVolFactorGoal + 0.01f)
                {
                    curRunVolFactor += Mathf.Sign (runHighGoal - runHigh) * time.DeltaTime / muffleSpeed;//
                    runVol = 
                }
                if (runHigh < runHighGoal - 1f || runHigh > runHighGoal + 1f)
                {
                    runHigh += (runHighGoal - runHigh) * time.DeltaTime / muffleSpeed;
                }
                if (runLow < runLowGoal - 1f || runLow > runLowGoal + 1f)
                {
                    runLow += Mathf.Sign (runLowGoal - runLow) * time.DeltaTime / muffleSpeed;
                }
            }
            else
            {
                // play shop music

            }
        }
        else
        {
            // play run music

        }
    }




    
    public void SetPauseEffect (bool pauseEffect)
    {
        if (interpolationCoroutine != null) StopCoroutine (interpolationCoroutine);

        if (pauseEffect)
        {
            interpolationCoroutine = StartCoroutine (InterpolateAudioEffect (5000f, 500f, 0.8f, 0.5f));
        }
        else
        {
            interpolationCoroutine = StartCoroutine (InterpolateAudioEffect (22000f, 10f, 1.25f, 0.5f));            
        }
    }

    private IEnumerator InterpolateAudioEffect (float targetLowpass, float targetHighpass, float volumeFactor, float duration)
    {
        // get current values of effects
        float lowpass, highpass, b1Vol, b2Vol, tutVol;
        mixer.GetFloat ("BGM Lowpass", out lowpass);
        mixer.GetFloat ("BGM Highpass", out highpass);
        mixer.GetFloat ("Base Music 1 Volume", out b1Vol);
        mixer.GetFloat ("Base Music 2 Volume", out b2Vol);
        mixer.GetFloat ("Tutorial Music Volume", out tutVol);

        // interpolate over time
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            // get next value for each effect
            float step = Mathf.SmoothStep (0f, 1f, elapsed / duration);
            float nextLowpass = Mathf.Lerp  (lowpass, targetLowpass, step);
            float nextHighpass = Mathf.Lerp (highpass, targetHighpass, step);
            float nextB1Vol = Mathf.Lerp (b1Vol, b1Vol * volumeFactor, step);
            float nextB2Vol = Mathf.Lerp (b2Vol, b2Vol * volumeFactor, step);
            float nextTutVol = Mathf.Lerp (tutVol, tutVol * volumeFactor, step);

            // set each effect to its next value
            mixer.SetFloat ("BGM Lowpass", nextLowpass);
            mixer.SetFloat ("BGM Highpass", nextHighpass);
            mixer.SetFloat ("Base Music 1 Volume", nextB1Vol);
            mixer.SetFloat ("Base Music 2 Volume", nextB2Vol);
            mixer.SetFloat ("Tutorial Music Volume", nextTutVol);

            yield return null;
        }

        // finally set everything to its target value
        mixer.SetFloat ("BGM Lowpass", targetLowpass);
        mixer.SetFloat ("BGM Highpass", targetHighpass);
        mixer.SetFloat ("Base Music 1 Volume", b1Vol * volumeFactor);
        mixer.SetFloat ("Base Music 2 Volume", b2Vol * volumeFactor);
        mixer.SetFloat ("Tutorial Music Volume", tutVol * volumeFactor);
    }

    public void SetMusicVolume (float volume)
    {
        mixer.SetFloat ("Master", volume == -30 ? -80 : volume);
    }

    public void SetSFXVolume (float volume)
    {
        sfxMixer.SetFloat ("Master", volume == -30 ? -80 : volume);
    }

    */
}
