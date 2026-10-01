using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using TMPro;
public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private TextMeshProUGUI MenuText;
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private TextMeshProUGUI TimeText;
    String sceneName;
    Boolean gameEnded = false;
    float timeSinceLoad ;
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
        //pauseMenu.GetComponent<Canvas>().enabled = false;
        LevelText.text = "Level: " + sceneName;
    }
    void Update()
    {
        Pause();
        timeSinceLoad = Time.timeSinceLevelLoad;
        TimeText.text = ""+ Math.Round(timeSinceLoad, 0);
    }
    void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameEnded )
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

    void Win(String l, float t)
    {
        gameEnded = true;
        DisplayPauseMenu("Win");
        LevelManager.Instance.UpdateLevelData(l, t);
        DatabaseManager.Instance.SaveLevelTime(l, "player", t);

        Time.timeScale = 0;
        
        StartCoroutine( WaitTimeThenLoad(2) );
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
}
