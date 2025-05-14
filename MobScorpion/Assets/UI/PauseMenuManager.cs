using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> menuItems;
    [SerializeField] private GameObject savePromptPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        if(savePromptPanel != null){
        savePromptPanel.SetActive(false);
        }
        
        foreach (GameObject g in menuItems)
        {
            if (g) g.SetActive(!g.activeSelf);
        }

        Time.timeScale = (Time.timeScale == 0f) ? 1f : 0f;
    }

    
    public void ShowSavePrompt()
    {
        savePromptPanel.SetActive(true);
    }

    public void OnSaveAndExit()
{
    StartCoroutine(SaveAndQuitCoroutine());
}

    public void OnExitWithoutSave()
    {
        Application.Quit();
    }

    private IEnumerator SaveAndQuitCoroutine()
{
    GameObject playerObj = GameObject.FindWithTag("Player1");
    if (playerObj == null)
    {
        Debug.LogError("Player1 not found");
        yield break;
    }

    Health player = playerObj.GetComponent<Health>();
    Camera cam = Camera.main;

    List<Health> enemies = new List<Health>();
    foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy"))
    {
        var h = go.GetComponent<Health>();
        if (h != null) enemies.Add(h);
    }

    SaveSystem.SaveGame(player, enemies, cam);
    Debug.Log("Saved game, now quitting");

#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif

    yield return null;
}


}
