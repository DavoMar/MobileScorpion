using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaryTextChild : MonoBehaviour
{

    public Image im;

    void Start ()
    {
        im.color = new Color (1f, 1f, 1f, 0f);
    }
   
    public void Display (Sprite sprite)
    {
        im.sprite = sprite;
    }

    public void Fade (float startTime, float displayTime, float fadeTime, float duration)
    {
        if (startTime + displayTime > Time.time)
            im.color += new Color (1f, 1f, 1f, Time.deltaTime / displayTime);
        else if (startTime + duration < Time.time)
            im.color = new Color (1f, 1f, 1f, 0f);
        else if (startTime + duration - fadeTime < Time.time)
            im.color += new Color (1f, 1f, 1f, -Time.deltaTime / fadeTime);
    }

}
