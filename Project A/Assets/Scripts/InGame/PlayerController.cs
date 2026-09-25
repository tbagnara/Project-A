using System.Numerics;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using System;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f; 
    [SerializeField] private float jumpHeight = 7f;
    private Rigidbody2D rb;
    private CharacterController controller;
    private float moveInput;
    public bool isGrounded;
    float dist = 0.01f;
    public static event Action<String> onLevelComplete;

    public event Action onDamage;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody2D>();        
    }

    void Update()
    {
        Move();
        Jump();
        
    }

    void FixedUpdate()
    {
        
    
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
    void OnTriggerEnter2D(Collider2D collision)
    {
        switch( collision.gameObject.tag.ToString() )
        {
            case "Spikes":
                Time.timeScale = 0;
                Time.timeScale = 1;
                SceneManager.LoadScene("MainMenus");
                break;
            case "Goal":
                Time.timeScale = 0;
                onLevelComplete?.Invoke(SceneManager.GetActiveScene().name );
                SceneManager.LoadScene("MainMenus");
                break;
        }

        
    }

}
