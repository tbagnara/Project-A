using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using TMPro;
public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject recordMenu;
    [SerializeField] private TextMeshProUGUI MenuText;
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private TextMeshProUGUI TimeText;
    [SerializeField] private TMP_InputField recordName;   // Input field text 
    String sceneName;
    private Boolean gameEnded = false;
    private float timeSinceLoad;

    private string recordLevel;
    private float recordTime;
    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        PlayerController.onLevelComplete += HandleWin;
        PlayerController.onLevelFail += HandleFailure;
    }

    void OnDisable()
    {
        PlayerController.onLevelComplete -= HandleWin;
        PlayerController.onLevelFail -= HandleFailure;

    }
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        LevelText.text = "Level: " + sceneName;
        AudioManager.Instance.PlayMusic(AudioManager.Instance.levelMusic);
    }
    void Update()
    {
        PauseMenu();
        timeSinceLoad = Time.timeSinceLevelLoad;
        TimeText.text = ""+ Math.Floor(timeSinceLoad);
    }
    void PauseMenu()
    {
        if (gameEnded) return;
        if ( Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace) )
        {
            Canvas pauseMenuCanvas = pauseMenu.GetComponent<Canvas>();
            if (pauseMenuCanvas.isActiveAndEnabled == false) 
            {
                DisplayPauseMenu("Pause");
                Time.timeScale = 0;
            }
            else
            {
                DisplayPauseMenu("Hide");
                Time.timeScale = 1;
            }
        }
    }
    void HandleFailure()
    {
        gameEnded = true;
        Time.timeScale = 0;
        DisplayPauseMenu("Failure");
        StartCoroutine( WaitTimeThenLoad(2) );
    }

    void HandleWin(String levelName)
    {
        gameEnded = true;
        Time.timeScale = 0;
        float levelCompletionTime = timeSinceLoad;
        
        LevelManager.Instance.UpdateLevelData(levelName, levelCompletionTime);
        
        float timeToBeat = DatabaseManager.Instance.GetTopTime(levelName); 
        if (levelCompletionTime < timeToBeat || timeToBeat == -1)   // Faster time or first clear
        {
            recordTime = levelCompletionTime;
            recordLevel = levelName;
            recordMenu.GetComponent<Canvas>().enabled = true;
        }
        else    // Slower time = no time recorded
        {
            DisplayPauseMenu("SlowWin");
            StartCoroutine( WaitTimeThenLoad(2) );
        }
        
    }

    IEnumerator WaitTimeThenLoad(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);

        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenus");
    }

    void DisplayPauseMenu(String s)    // Brings UI (pauseMenu) ontoscreen, changes text based on which menu is in use
    {
        switch (s) 
        {
            case "Pause": 
                pauseMenu.transform.position = new UnityEngine.Vector3(0, 0, -1);
                break;
            case "Failure":
                MenuText.text = "- - You Died - -";  
                break;
            case "SlowWin": // Won but not record time
                MenuText.text = "- Level Cleared -";
                break;
            default:
                pauseMenu.GetComponent<Canvas>().enabled = false;
                return;
        }
        pauseMenu.GetComponent<Canvas>().enabled = true;

    }

    public void RecordPlayerName(String name) // Triggered when input field is exited
    {
        
        string playerName = recordName.text;
        if (playerName.Length > 5) 
        {
            playerName = playerName.Substring(0,5);
        }
        DatabaseManager.Instance.SaveLevelTime(recordLevel, playerName, recordTime);
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenus");
    }
    public Boolean IsGameOver()
    {
        return gameEnded;
    }
    
}
