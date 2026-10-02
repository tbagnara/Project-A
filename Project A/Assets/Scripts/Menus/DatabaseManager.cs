using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Events;
using System;
using System.Linq;

[System.Serializable]
public class PlayerTimeData
{
    public List<HighScore> levelTimes = new List<HighScore>();

}

[System.Serializable]
public class HighScore // Save data (Without sqlite)
{
    public string PlayerLevel;
    public string PlayerName;
    public float CompletionTime;

    public HighScore(string l, string n, float t)
    {
        PlayerLevel = l;
        PlayerName = n;
        CompletionTime = t;
    }
}

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }


    private PlayerTimeData playerData;
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
        
        
        savePath = Path.Combine(Application.persistentDataPath, "playertime.json");
        LoadPlayerData();
        
        
    }
    
    
    public void SaveLevelTime(string level, string playerName, float completionTime)
    {

        HighScore newScore = new HighScore(level, playerName, completionTime);
        SaveTime(newScore);
        
        Debug.Log("High score saved: " + level + " - " + playerName + " - " + completionTime);
    }
    public float GetTopTime(string level)
    {
        float time;
        try
        {
            List<HighScore> topScores = playerData.levelTimes
            .Where(x => x.PlayerLevel.Equals(level))
            .OrderBy(time => time.CompletionTime)
            .Take(1)
            .ToList();

            time = topScores[0].CompletionTime;
        }
        catch
        {
            time = -1;
        }
            
        return time;
    }

    public String GetTopPlayer(string level)
    {
        String player;
        try
        {
            List<HighScore> topScores = playerData.levelTimes
            .Where(x => x.PlayerLevel.Equals(level))
            .OrderBy(time => time.CompletionTime)
            .Take(1)
            .ToList();

            player = topScores[0].PlayerName;
        }
        catch
        {
            player = "-----";
        }
            
        return player;
    }




    public void SavePlayerData()
    {
        savePath = Path.Combine(Application.persistentDataPath, "playertime.json");
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
                
                playerData = JsonUtility.FromJson<PlayerTimeData>(json);
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
        playerData = new PlayerTimeData();
        SavePlayerData();
    }
    
    public void SaveTime(HighScore levelTimeData)
    {
        playerData.levelTimes.Add(levelTimeData);
        SavePlayerData();
    }

    void OnApplicationQuit()
    {
        SavePlayerData();
    }

}
    





