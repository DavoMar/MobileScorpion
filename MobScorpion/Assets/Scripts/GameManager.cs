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
        yield return new WaitForSeconds(0.1f); // Give time for everything to initialize

        string currentScene = SceneManager.GetActiveScene().name;

        if (!SaveSystem.SaveExists())
            yield break;

        if ((currentScene == "Temple of The Scorpion") || (currentScene == "Main" && !SaveZoneTrigger.canSave))
        {
            SaveSystem.DeleteSave();
            yield break;
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
        SaveData data = SaveSystem.LoadGame();
        if (data == null) return;

        player.transform.position = data.playerPosition;
        player.currHealth = data.playerHealth;

        Lives lives = player.GetComponent<Lives>();
        if (lives != null)
            lives.currentLives = data.playerLives;

        PlayerBuy buy = player.GetComponent<PlayerBuy>();
        if (buy != null && buy.coinManager != null)
            buy.coinManager.player1Coins = data.playerCoins;

        if (MainCamera != null)
            MainCamera.transform.position = data.cameraPosition;
    }
}
