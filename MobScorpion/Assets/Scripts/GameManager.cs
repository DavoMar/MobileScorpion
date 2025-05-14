using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Health player;
    public Camera MainCamera;

    void OnApplicationPause(bool pause)
    {
        if (pause && SaveZoneTrigger.canSave)
        {
            SaveGameData();
        }
    }

    void OnApplicationQuit()
    {
        if (SaveZoneTrigger.canSave)
        {
            SaveGameData();
        }
    }

    IEnumerator Start()
{
    yield return new WaitForSeconds(0.2f); // Slight delay for scene to fully load

    string currentScene = SceneManager.GetActiveScene().name;

    if (!SaveSystem.SaveExists())
        yield break;

    if ((currentScene == "Temple of The Scorpion") || (currentScene == "Main" && !SaveZoneTrigger.canSave))
    {
        SaveSystem.DeleteSave();
        yield break;
    }

    // Wait until Player1 is found in the scene
    while (GameObject.FindWithTag("Player1") == null)
    {
        yield return null;
    }

    LoadGame();
}


        public void SaveGameManually()
    {
        if (!SaveZoneTrigger.canSave) return;

        List<Health> enemies = new List<Health>();
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            var h = go.GetComponent<Health>();
            if (h != null) enemies.Add(h);
        }

        SaveSystem.SaveGame(player, enemies, MainCamera);
        Debug.Log("Manual save triggered from pause menu.");
    }

    void SaveGameData()
    {
        List<Health> enemies = new List<Health>();
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Health h = go.GetComponent<Health>();
            if (h != null)
                enemies.Add(h);
        }

        SaveSystem.SaveGame(player, enemies, MainCamera);
    }

    void LoadGame()
{
    var data = SaveSystem.LoadGame();
    if (data == null)
    {
        Debug.LogError("Save data is null");
        return;
    }

    GameObject playerObj = GameObject.FindWithTag("Player1");
    if (playerObj == null)
    {
        Debug.LogError("Player1 not found");
        return;
    }

    Health player = playerObj.GetComponent<Health>();
    player.transform.position = data.playerPosition;
    player.HealDamage(data.playerHealth - player.GetHealth());
    Debug.Log("Loaded player at position " + data.playerPosition + " with health " + data.playerHealth);

    // Load Lives and Coins if stored
    if (player.totalLives != null)
    {
        player.totalLives.currentLives = data.playerLives;
        Debug.Log("Loaded lives: " + data.playerLives);
    }

    var buy = player.GetComponent<PlayerBuy>();
    if (buy != null && buy.coinManager != null)
    {
        buy.coinManager.player1Coins = data.playerCoins;
        Debug.Log("Loaded coins: " + data.playerCoins);
    }

    if (data.cameraPosition != Vector3.zero && Camera.main != null)
    {
        Camera.main.transform.position = data.cameraPosition;
        Debug.Log("Loaded camera position: " + data.cameraPosition);
    }

    GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");

    for (int i = 0; i < Mathf.Min(enemyObjects.Length, data.enemies.Count); i++)
    {
        var h = enemyObjects[i].GetComponent<Health>();
        if (h != null)
        {
            enemyObjects[i].transform.position = data.enemies[i].position;
            h.HealDamage(data.enemies[i].health - h.GetHealth());
        }
    }
}

}
