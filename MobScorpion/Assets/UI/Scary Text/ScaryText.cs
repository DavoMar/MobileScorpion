using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaryText : MonoBehaviour
{
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip muahahaha1;
    [SerializeField] private AudioClip muahahaha2;
    [SerializeField] private AudioClip gong;
    [SerializeField] private List<Sprite> enteringText;
    [SerializeField] private List<float> enteringTextDuration;
    [SerializeField] private Sprite childEnteringSprite;
    [SerializeField] private List<Sprite> treasureRoomText;
    [SerializeField] private List<float> treasureRoomTextDuration;
    [SerializeField] private Sprite childTreasureRoomSprite;
    [SerializeField] private float displayTime = 0.5f;
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private BGMController bgmController;
    [SerializeField] private Toggle toggle;

    public bool isNotFinalFight = true;

    private Coroutine sequenceCoroutine;
    private Image im;
    private RectTransform rectTransform;
    private ScaryTextChild child;
    private int displayText; // 0 represents false, 1 represents true
    private bool setup = true; // don't run the ToggleDisplayText() method during setup

    void Start ()
    {
        im = GetComponent<Image> ();
        im.color = new Color (1f, 1f, 1f, 0f);
        rectTransform = GetComponent<RectTransform> ();
        child = GetComponentInChildren<ScaryTextChild> ();

        if (PlayerPrefs.HasKey ("DisplayText"))
        {
            displayText = PlayerPrefs.GetInt ("DisplayText");
            toggle.isOn = displayText == 1;
        }
        else
        {
            PlayerPrefs.SetInt ("DisplayText", 1);
        }
        setup = false;
    }
    
    public void PlayEnteringSequence ()
    {
        if (displayText == 1)
            sequenceCoroutine = StartCoroutine (EnteringSequence ());
    }

    public void PlayTreasureRoomSequence ()
    {
        if (displayText == 1)
            sequenceCoroutine = StartCoroutine (TreasureRoomSequence ());
    }

    private IEnumerator EnteringSequence ()
    {
        float duration = 0f;
        float startTime = float.NegativeInfinity;
        int i = 0;
        while (true)
        {
            if (startTime + duration < Time.time)
            {
                // finish fade
                Fade (startTime, displayTime, fadeTime, duration);
                if (i == 1)
                    child.Fade (startTime, displayTime, fadeTime, duration);

                if (i == enteringTextDuration.Count)
                    break;
                src.PlayOneShot (gong);
                if (i == 2)
                    src.PlayOneShot (muahahaha2);
                duration = enteringTextDuration[i];
                startTime = Time.time;
                Display (enteringText[i]);
                child.Display (childEnteringSprite);
                ++i;
            }
            else
            {
                Fade (startTime, displayTime, fadeTime, duration);
                if (i == 1)
                    child.Fade (startTime, displayTime, fadeTime, duration);
            }

            yield return null;
        }

        // finish fade
        Fade (startTime, displayTime, fadeTime, duration);
        if (i == 1)
            child.Fade (startTime, displayTime, fadeTime, duration);
    }

    private IEnumerator TreasureRoomSequence ()
    {
        float duration = 0f;
        float startTime = float.NegativeInfinity;
        int i = 0;
        while (true)
        {
            if (startTime + duration < Time.time)
            {
                //finish fade
                Fade (startTime, displayTime, fadeTime, duration);
                if (i == 1)
                    child.Fade (startTime, displayTime, fadeTime, duration);

                if (i == treasureRoomTextDuration.Count)
                    break;
                if (i == 1)
                    bgmController.BeginFinalFight ();
                src.PlayOneShot (gong);
                if (i == 3)
                {
                    src.PlayOneShot (muahahaha1);
                    isNotFinalFight = false;
                }
                duration = treasureRoomTextDuration[i];
                startTime = Time.time;
                Display (treasureRoomText[i]);
                child.Display (childTreasureRoomSprite);
                ++i;
            }
            else
            {
                Fade (startTime, displayTime, fadeTime, duration);
                if (i == 1)
                    child.Fade (startTime, displayTime, fadeTime, duration);
            }

            yield return null;
        }

        // finish fade
        Fade (startTime, displayTime, fadeTime, duration);
        if (i == 1)
            child.Fade (startTime, displayTime, fadeTime, duration);
    }

    private void Display (Sprite sprite)
    {
        im.sprite = sprite;
    }

    private void Fade (float startTime, float displayTime, float fadeTime, float duration)
    {
        if (startTime + displayTime > Time.time)
            im.color += new Color (1f, 1f, 1f, Time.deltaTime / displayTime);
        else if (startTime + duration < Time.time)
            im.color = new Color (1f, 1f, 1f, 0f);
        else if (startTime + duration - fadeTime < Time.time)
            im.color += new Color (1f, 1f, 1f, -Time.deltaTime / fadeTime);
    }

    public void ToggleDisplayText ()
    {
        if (setup)
            return;

        if (displayText == 1)
            displayText = 0;
        else
            displayText = 1;
        PlayerPrefs.SetInt ("DisplayText", displayText);
    }

}
