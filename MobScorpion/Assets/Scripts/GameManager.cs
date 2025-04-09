using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameBootstrap : MonoBehaviour
{
    public Health player;
    
    void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        // Don't load save for intro scene or first zone of scene 2
        if ((currentScene == "Temple of The Scorpion") || (currentScene == "Main" && IsFirstZone()) || !SaveSystem.SaveExists())
        {
            SaveSystem.DeleteSave(); // Clear any old saves
            return;
        }

        LoadGame();
    }

    void OnApplicationQuit()
    {
        List<Health> enemies = new List<Health>();
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            var h = go.GetComponent<Health>();
            if (h != null) enemies.Add(h);
        }

        SaveSystem.SaveGame(player, enemies);
    }

    void LoadGame()
    {
        var data = SaveSystem.LoadGame();
        if (data == null) return;

        // Load player
        player.transform.position = data.playerPosition;
        player.HealDamage(data.playerHealth - player.GetHealth()); // Adjust health

        // Load enemies
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

    private bool IsFirstZone()
    {
        return false; 
    }
}
