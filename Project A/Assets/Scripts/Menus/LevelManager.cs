using System;
using System.Collections;
using System.Numerics;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public Boolean[] levelsBeaten;
    public Boolean GameStarted;
    public int levelSelected;
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

    void OnEnable()
    {
        PlayerController.onLevelComplete += UpdateLevelData;
        levelSelected = 0;
        levelsBeaten = new Boolean[16];
        for (int i = 0; i< levelsBeaten.Length; i++)
        {
            levelsBeaten[i] = false;
        }
    }

    void OnDisable()
    {
        PlayerController.onLevelComplete -= UpdateLevelData;
    }

    void UpdateLevelData(String l)
    {
        Time.timeScale = 1;
        char world = l[0];
        int worldNum = world - '0';
        char level = l[l.Length-1];
        int levelNum = level - '0';

        levelsBeaten[ (worldNum - 1) * 8 + levelNum - 1] = true;        
        
    }
    
    void OnStart()
    {
        
    }

    void Update()
    {
        
    }

    public Boolean isBeaten(int w, int l)
    {
        int levelNo = ((w-1)*8) + l - 1;
        if (levelsBeaten[levelNo] )
            return true;
        return false;
    }
    public Boolean isBeaten(String str)
    {
        UnityEngine.Vector2 num = convertToInt(str);
        int world = (int) num.x;
        int level = (int) num.y;
        return isBeaten(world, level);

    }
    public Boolean isAvailable(int w, int l)
    {
        if (w == 1 && l == 1) return true;
        if (l == 1)
        {
            w -= 1; 
            l = 8;
        }
        else
        {
            l -=1;
        }
        return isBeaten(w, l);
    }
    public Boolean isAvailable(String str)
    {
        UnityEngine.Vector2 num = convertToInt(str);
        int world = (int) num.x;
        int level = (int) num.y;
  
        return isAvailable(world, level);

    }

    public UnityEngine.Vector2 convertToInt(String l)
    {
        char world = l[0];
        int worldNum = world - '0';
        char level = l[l.Length-1];
        int levelNum = level - '0';
        

        return new UnityEngine.Vector2(worldNum, levelNum);
    }
}
