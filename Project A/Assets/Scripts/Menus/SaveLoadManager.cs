using System;
using UnityEngine;
using System.IO;

[System.Serializable]
public class PlayerSaveData
{
    public Boolean[] levelsBeaten;

    public string lastPlayed;

}

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance { get; private set; }
    
    private PlayerSaveData playerData;
    private string savePath;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        savePath = Path.Combine(Application.persistentDataPath, "playersave.json");
        LoadPlayerData();
    }
    public void SavePlayerData()
    {
        playerData.lastPlayed = System.DateTime.Now.ToString();
        string json = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(savePath, json);
        Application.ExternalEval("_JS_FileSystem_Sync();");
        Debug.Log("Player data saved!");
    }
    
    public void LoadPlayerData()
    {
        if (File.Exists(savePath))
        {
            try
            {
                string json = File.ReadAllText(savePath);
                playerData = JsonUtility.FromJson<PlayerSaveData>(json);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Load failed: " + e.Message);
                CreateNewPlayerData();
            }
        }
        else
        {
            CreateNewPlayerData();
        }
    }
    
    void CreateNewPlayerData()      // Create savedata
    {
        playerData = new PlayerSaveData();
        playerData.levelsBeaten = new Boolean[16];
        for (int i = 0; i< playerData.levelsBeaten.Length; i++)
        {
            playerData.levelsBeaten[i] = false;
        }
        SavePlayerData();
    }
    
    public void SaveData(Boolean[] levelData)
    {
        playerData.levelsBeaten = levelData;
        SavePlayerData();
    }

    public bool[] GetLevelsBeatenData() { return playerData.levelsBeaten; }

    void OnApplicationQuit()
    {
        SavePlayerData();
    }
}