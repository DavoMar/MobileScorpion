using System.IO;
using UnityEngine;
using System.Collections.Generic;

public static class SaveSystem
{
    private static string path = Application.persistentDataPath + "/save.json";

    public static void SaveGame(Health player, List<Health> enemies, Camera cam)
{
    SaveData data = new SaveData();
    data.sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    data.playerPosition = player.transform.position;
    data.playerHealth = player.GetHealth();

    Debug.Log("Saving player position: " + data.playerPosition);
    Debug.Log("Saving player health: " + data.playerHealth);

    PlayerBuy buy = player.GetComponent<PlayerBuy>();
    if (buy != null)
    {
        data.playerCoins = buy.coinManager?.player1Coins ?? 0;
        Debug.Log("Saving player coins: " + data.playerCoins);
    }

    if (player.totalLives != null)
    {
        data.playerLives = player.totalLives.currentLives;
        Debug.Log("Saving player lives: " + data.playerLives);
    }

    if (cam != null)
    {
        data.cameraPosition = cam.transform.position;
        Debug.Log("Saving camera position: " + data.cameraPosition);
    }

    foreach (var enemy in enemies)
    {
        EnemyData ed = new EnemyData();
        ed.position = enemy.transform.position;
        ed.health = enemy.GetHealth();
        data.enemies.Add(ed);
        Debug.Log("Saving enemy at " + ed.position + " with health " + ed.health);
    }

    string json = JsonUtility.ToJson(data, true);
    
    try
    {
        File.WriteAllText(path, json);
        Debug.Log("Game saved successfully to " + path);
    }
    catch (IOException e)
    {
        Debug.LogError("Failed to save game: " + e.Message);
    }
}



    public static SaveData LoadGame()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<SaveData>(json);
        }
        return null;
    }

    public static void DeleteSave()
    {
        if (File.Exists(path))
            File.Delete(path);
    }

    public static bool SaveExists()
    {
        return File.Exists(path);
    }
}
