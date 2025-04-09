using System.IO;
using UnityEngine;
using System.Collections.Generic;

public static class SaveSystem
{
    private static string path = Application.persistentDataPath + "/save.json";

    public static void SaveGame(Health player, List<Health> enemies)
    {
        SaveData data = new SaveData();
        data.sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        data.playerPosition = player.transform.position;
        data.playerHealth = player.GetHealth();

        foreach (var enemy in enemies)
        {
            EnemyData ed = new EnemyData();
            ed.position = enemy.transform.position;
            ed.health = enemy.GetHealth();
            data.enemies.Add(ed);
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        Debug.Log("Game saved to " + path);
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