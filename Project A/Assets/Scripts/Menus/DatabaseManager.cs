using UnityEngine;
using SQLite;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Events;

public class HighScore
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string PlayerLevel { get; set; }
    public string PlayerNumber { get; set; }
    public float CompletionTime { get; set; }
}

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }
    
    private string dbPath;
    private SQLiteConnection dbConnection;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        SetDatabasePath();
        InitializeDatabase();
        
    }
    
    void SetDatabasePath()
    {
        dbPath = Path.Combine(Application.persistentDataPath, "gamedata.db");
    }
    
    void InitializeDatabase()
    {
        dbConnection = new SQLiteConnection(dbPath);
        CreateLevelScoresTable();
    }
    
    void CreateLevelScoresTable()
    {
        dbConnection.CreateTable<HighScore>();
        Debug.Log("High Scores table created at: " + dbPath);
    }
    public void SaveLevelTime(string level, string playerNumber, float completionTime)
    {
        HighScore newScore = new HighScore
        {
            PlayerLevel = level,
            PlayerNumber = playerNumber,
            CompletionTime = completionTime
        };
        
        dbConnection.Insert(newScore);
        Debug.Log("High score saved: " + level + "-" + playerNumber + " - " + completionTime);
    }
    public HighScore GetTopTime(string level)
    {
        
        List<HighScore> topScores = dbConnection.Table<HighScore>()
            .OrderBy(time => time.CompletionTime)
            .Take(1)
            .Where(x => x.PlayerLevel.Equals(level))
            .ToList();
        
        return topScores[0];
    }

}