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

    public void exitGame()
{
    // Optionally, pause time and/or hide menu items here
    GameManager bootstrap = FindObjectOfType<GameManager>();
    if (bootstrap != null)
    {
        bootstrap.SaveGameManually(); // Call a manual save method you create in GameManager
    }

    Application.Quit(); // Closes the app
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