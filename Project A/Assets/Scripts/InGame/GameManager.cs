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
    String sceneName;
    Boolean gameEnded = false;
    float timeSinceLoad ;
    [SerializeField] private TMP_InputField recordName;
    String recordLevel;
    float recordTime;
    void Awake()
    {
        if(Instance!=null && Instance !=this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        //DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        PlayerController.onLevelComplete += Win;
        PlayerController.onLevelFail += Failure;
    }

    void OnDisable()
    {
        PlayerController.onLevelComplete -= Win;
        PlayerController.onLevelFail -= Failure;

    }
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        LevelText.text = "Level: " + sceneName;
        AudioManager.Instance.PlayMusic(AudioManager.Instance.levelMusic);
    }
    void Update()
    {
        Pause();
        timeSinceLoad = Time.timeSinceLevelLoad;
        TimeText.text = ""+ Math.Floor(timeSinceLoad);
    }
    void Pause()
    {
        if ( (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace) )&& !gameEnded )
        {
            if (pauseMenu.GetComponent<Canvas>().isActiveAndEnabled == false)
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
    void Failure()
    {
        gameEnded = true;
        DisplayPauseMenu("Failure");
        Time.timeScale = 0;
        

        StartCoroutine( WaitTimeThenLoad(2) );
    }

    void Win(String l)
    {
        gameEnded = true;
        Time.timeScale = 0;
        float t = timeSinceLoad;
        
        LevelManager.Instance.UpdateLevelData(l, t);
        
        float timeToBeat = DatabaseManager.Instance.GetTopTime(l);
        if (t < timeToBeat || timeToBeat == -1)
        {
            recordTime = t;
            recordLevel = l;
            DisplayRecordTimeMenu();
        }
        else
        {
            DisplayPauseMenu("Win");
            StartCoroutine( WaitTimeThenLoad(2) );
        }
        
    }

    public Boolean IsGameOver()
    {
        return gameEnded;
    }

    IEnumerator WaitTimeThenLoad(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);

        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenus");
    }


    public float getTimeSinceLoad()
    {
        return timeSinceLoad;
    }

    void DisplayPauseMenu(String s)    // Brings UI ontoscreen
    {
        switch (s) 
        {
            case "Pause": 
                pauseMenu.transform.position = new UnityEngine.Vector3(0, 0, -1);
                break;
            case "Failure":
                MenuText.text = "- - You Died - -";  
                break;
            case "Win":
                MenuText.text = "- Level Cleared -";
                break;
            default:
                pauseMenu.GetComponent<Canvas>().enabled = false;
                return;
        }
        pauseMenu.GetComponent<Canvas>().enabled = true;

    }

    public void RecordPlayerName(String name)
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
    void DisplayRecordTimeMenu()
    {
        recordMenu.GetComponent<Canvas>().enabled = true;
    }
}
