using System.IO;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    private string savePath;
    
    void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "save.json");
    }
    
    public void SaveGame(GameData data)
    {
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, jsonData);
    }
    
    public GameData LoadGame()
    {
        if (File.Exists(savePath))
        {
            string jsonData = File.ReadAllText(savePath);
            return JsonUtility.FromJson<GameData>(jsonData);
        }
        return new GameData();
    }
}

[System.Serializable]
public class GameData
{
    // 存档数据结构
    public int playerLevel;
    public float playerHealth;
    public string[] inventoryItems;
}