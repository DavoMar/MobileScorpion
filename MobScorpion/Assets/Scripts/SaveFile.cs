using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string sceneName;
    public Vector3 playerPosition;
    public int playerHealth;

    public List<EnemyData> enemies = new List<EnemyData>();
}

[System.Serializable]
public class EnemyData
{
    public Vector3 position;
    public int health;
}
