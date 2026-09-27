using System.Numerics;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
//using UnityEngine.InputSystem;
using System;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f; 
    [SerializeField] private float jumpHeight = 7f;
    private Rigidbody2D rb;
    private CharacterController controller;
    private float moveInput;
    public bool isGrounded;
    private bool ended;
    float dist = 0.01f;
    public static event Action<String> onLevelComplete;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private TextMeshProUGUI MenuText;

    public event Action onDamage;
    void Start()
    {
        pauseMenu.transform.position = new UnityEngine.Vector3(-100, 0, -1);
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody2D>();    
        Time.timeScale = 1;    
    }

    void Update()
    {
        Move();
        Jump();
        Pause();
    }

    void FixedUpdate()
    {
        
    
    }

    void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !ended)
        {
            if (Time.timeScale == 1)
            {
                MenuText.text = "- - - Paused - - -";
                pauseMenu.transform.position = new UnityEngine.Vector3(0, 0, -1);
                Time.timeScale = 0;
            }
            else
            {
                pauseMenu.transform.position = new UnityEngine.Vector3(-100, 0, -1);
                Time.timeScale = 1;
            }
        }
    }

    void Move()
    {
        
        float moveInput = Input.GetAxis("Horizontal");

        rb.linearVelocityX = moveInput * moveSpeed;
        
    }

    void Jump()
    {
        isGrounded = Physics2D.CircleCast(transform.position, 0.25f, UnityEngine.Vector2.down, dist);
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocityY = jumpHeight;   
        }
    }

    //void OnCollisionExit2D(Collision2D collision)
    IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        switch( collision.gameObject.tag.ToString() )
        {
            case "Spikes":
                ended = true;
                Time.timeScale = 0;
                MenuText.text = "- - You Died - -";
                pauseMenu.transform.position = new UnityEngine.Vector3(0, 0, -1);
                yield return new WaitForSecondsRealtime(2f);
                
                SceneManager.LoadScene("MainMenus");
                break;
            case "Goal":
                ended = true;
                Time.timeScale = 0;
                MenuText.text = "- Level Cleared -";
                pauseMenu.transform.position = new UnityEngine.Vector3(0, 0, -1);
                yield return new WaitForSecondsRealtime(2f);

                onLevelComplete?.Invoke(SceneManager.GetActiveScene().name );
                SceneManager.LoadScene("MainMenus");
                break;
        }

        
    }

}
