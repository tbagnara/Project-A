using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Globalization;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public Camera StartMenuCamera;
    public Camera LevelSelectCamera;
    private int levelSelected;
    public TextMeshProUGUI levelText;
    [SerializeField] public List<GameObject> Levels;

    void Start()
    {
        levelSelected = LevelManager.Instance.levelSelected;
        UpdateLevelText();
        transform.position = new Vector2(Levels.ElementAt(levelSelected).transform.position.x, transform.position.y);
        StartMenuCamera.enabled = true;
        LevelSelectCamera.enabled = false;
        LevelSelectCamera.GetComponent<AudioListener>().enabled = false;
        
        if (LevelManager.Instance.GameStarted) {
            StartMenuCamera.GetComponent<AudioListener>().enabled = false;
            StartMenuCamera.enabled = false;
            LevelSelectCamera.enabled = true;
            LevelSelectCamera.GetComponent<AudioListener>().enabled = true;
        }
    }

    void Update()
    {
        InputForMove();
        Move();
        Select();
    }

    void Select()
    {
        if (Input.GetKeyDown(KeyCode.Space) )
        {
            LevelManager.Instance.levelSelected = levelSelected;
            SceneManager.LoadScene(""+(levelSelected/8 + 1) + "-" + (levelSelected%8 + 1));
        }
        if (!LevelManager.Instance.GameStarted) 
            LevelManager.Instance.GameStarted = true;
    }


    void Pause()
    {
        if ( Input.GetButtonDown("Cancel") )
        {
            
        }
    }

    public void LevelSelectScreen()
    {
        StartMenuCamera.GetComponent<AudioListener>().enabled = false;
        StartMenuCamera.enabled = false;
        LevelSelectCamera.enabled = true;
        LevelSelectCamera.GetComponent<AudioListener>().enabled = true;
        //StartMenuCamera.transform.position = new Vector3(0, -10, -10);
    }

    public void InputForMove()
    {
        if (!LevelSelectCamera.isActiveAndEnabled) return;
        
        if (Input.GetKeyDown(KeyCode.A) && levelSelected !=0)
        {
            levelSelected--;
            UpdateLevelText();
        }
        else if (Input.GetKeyDown(KeyCode.D) && LevelManager.Instance.levelsBeaten[levelSelected] )
        {
            levelSelected++;
            UpdateLevelText();
        }

    }

    public void Move()
    {
        try {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(Levels.ElementAt(levelSelected).transform.position.x, transform.position.y) , 10*Time.deltaTime);
        }
        catch
        {
            
        }
    }

    public void UpdateLevelText()
    {
        levelText.text = "Level: " + (levelSelected/8 + 1) + " - " + (levelSelected%8 + 1);
    }

}

