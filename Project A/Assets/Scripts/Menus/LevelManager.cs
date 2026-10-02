using System;
using System.Collections;
using System.Numerics;
using NUnit.Framework.Internal;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public Boolean[] levelsBeaten;
    public int levelSelected = 0;

    void Awake()
    {
        if(Instance!=null && Instance !=this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        LoadData( SaveLoadManager.Instance.GetLevelsBeatenData() );
    }
    public void LoadData(Boolean[] data)
    {
        levelsBeaten = data;
    }

    public void UpdateLevelData(String l, float t)  // Saves level completion and time
    {
        UnityEngine.Vector2 worldLevel = convertWorldStringToInt(l);   
        int worldNum = (int) worldLevel.x;
        int levelNum = (int) worldLevel.y;
        int levelValue = (worldNum - 1) * 8 + levelNum - 1; //  Converts world and level integers into a single value determing which level it is in order

        levelsBeaten[ levelValue ] = true;    
        SaveLoadManager.Instance.SaveData(levelsBeaten);
    }

    public Boolean isBeaten(int w, int l) // Determines if a level has been beaten
    {
        int levelNo = ((w-1)*8) + l - 1;
        if (levelsBeaten[levelNo] )
            return true;
        return false;
    }
    public Boolean isBeaten(String str) // Helper
    {
        UnityEngine.Vector2 num = convertWorldStringToInt(str);
        int world = (int) num.x;
        int level = (int) num.y;
        return isBeaten(world, level);

    }
    public Boolean isAvailable(int w, int l)    // Determines if a level has been unlocked
    {
        if (w == 1 && l == 1) return true; // If level 1, automatically unlocked
        if (l == 1) // If level 1 of a different world, checks the last level of the previous world
        {
            w -= 1; 
            l = 8;
        }
        else
        {
            l -=1; // Checks previous level
        }
        return isBeaten(w, l);
    }
    public Boolean isAvailable(String str) // Helper
    {
        UnityEngine.Vector2 num = convertWorldStringToInt(str);
        int world = (int) num.x;
        int level = (int) num.y;
  
        return isAvailable(world, level);

    }
    public UnityEngine.Vector2 convertWorldStringToInt(String l)   // Converts the string level name into an a vector containing integers of world and level
    {
        char world = l[0];
        int worldNum = world - '0';
        char level = l[l.Length-1];
        int levelNum = level - '0';
        

        return new UnityEngine.Vector2(worldNum, levelNum);
    }
    public Boolean[] getLevelsBeaten()
    {
        return levelsBeaten;
    }

}
