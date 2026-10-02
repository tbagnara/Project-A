using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Globalization;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq.Expressions;
using Unity.VisualScripting;

public class MenuManager : MonoBehaviour
{

    public GameObject StartMenuScreen;  // Screen includes camera and canvas
    public GameObject LevelSelectScreen;
    public GameObject CreditsScreen;
    public TextMeshProUGUI levelText;
    [SerializeField] public List<GameObject> Levels;
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private TextMeshProUGUI player;
    private int levelSelected;
    private Boolean enteredLevelScreen = false;
    void Start()
    {
        levelSelected = LevelManager.Instance.levelSelected;
        UpdateLevelText();
        UpdateTimeText();

        if (Time.unscaledTime < 4)
        {
            enteredLevelScreen = false;
        }
        
        if (enteredLevelScreen == false) // Determines if the game has jsut been opened - if so boot into the main menu, else the level select screen
        {
            SetCamera("StartMenu");
        }
        else {
            SetCamera("LevelMenu");
        }
        AudioManager.Instance.PlayMusic(AudioManager.Instance.backgroundMusic);
    }

    void Update()
    {
        MoveInput();
        MoveLevelSelectCharacter();
        SelectLevel();
    }

    void SelectLevel()
    {
        if (Input.GetKeyDown(KeyCode.Space) )
        {
            LevelManager.Instance.levelSelected = levelSelected;
            SceneManager.LoadScene(""+(levelSelected/8 + 1) + " - " + (levelSelected%8 + 1));
        }
    }
    public void MoveInput() // Gets input, changes which level is selected
    {
        if (enteredLevelScreen == false) return;
        
        if (Input.GetKeyDown(KeyCode.A) && levelSelected%8 !=0)
        {
            levelSelected--;
        }
        else if (Input.GetKeyDown(KeyCode.D) && LevelManager.Instance.levelsBeaten[levelSelected] && (levelSelected +1 ) != Levels.Count)
        {
            levelSelected++;
        } 
        UpdateLevelText();
        UpdateTimeText();

    }

    public void MoveLevelSelectCharacter()  // Changes the position of character
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(Levels.ElementAt(levelSelected).transform.position.x, transform.position.y) , 10*Time.deltaTime);
    }

    public void UpdateLevelText()
    {
        levelText.text = "" + (levelSelected/8 + 1) + " - " + (levelSelected%8 + 1);
    }

    public void UpdateTimeText()
    {
        String level = levelText.text;
        float fastestTime = DatabaseManager.Instance.GetTopTime(level);
        fastestTime = (float)Math.Round(fastestTime, 1); 
        
        if (fastestTime >-1)
        {
            time.text = ""+fastestTime + "s";
        }
        else
        {
            time.text = "---";
        }

        String fastestPlayer = DatabaseManager.Instance.GetTopPlayer(levelText.text);

        player.text = fastestPlayer;

    }
    public void SetCamera(String cam)
    {
        LevelSelectScreen.SetActive(false);
        StartMenuScreen.SetActive(false);
        CreditsScreen.SetActive(false);

        switch (cam)
        {
            case "StartMenu":
                StartMenuScreen.SetActive(true);
                break;
            
            case "LevelMenu":
                enteredLevelScreen = true;
                LevelSelectScreen.SetActive(true);
                break;

            case "CreditsMenu":
                CreditsScreen.SetActive(true);
                break;       
        }

    }

}

