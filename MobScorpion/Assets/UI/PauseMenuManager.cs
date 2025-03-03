using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{

    [SerializeField] private List<GameObject> menuItems;

    void Update()
    {
        if (Input.GetKeyDown (KeyCode.Space))
        {
            foreach (GameObject g in menuItems)
            {
                if (g)
                    g.SetActive (!g.activeSelf);
            }
            if (Time.timeScale == 0f)   Time.timeScale = 1f;
            else Time.timeScale = 0f; 
        }
    }

    public void exitGame(){
        Application.Quit();
    }

    public void pauseorResumeGame(){
            foreach (GameObject g in menuItems)
            {
                if (g)
                    g.SetActive (!g.activeSelf);
            }
            if (Time.timeScale == 0f)   Time.timeScale = 1f;
            else Time.timeScale = 0f; 
    }
}

