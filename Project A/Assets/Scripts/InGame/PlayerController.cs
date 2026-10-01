using System.Numerics;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using System;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f; 
    [SerializeField] private float jumpHeight = 7f;
    private Rigidbody2D rb;
    private CharacterController controller;
    public ContactFilter2D ContactFilterDown;
    public ContactFilter2D ContactFilterLeft;
    public ContactFilter2D ContactFilterRight;


    public static event Action<String, float> onLevelComplete;
    public static event Action onLevelFail;
    float timeSinceLoad;
    public bool IsGrounded => rb.IsTouching(ContactFilterDown);
    public bool IsTouchingRight => rb.IsTouching(ContactFilterRight);
    public bool IsTouchingLeft => rb.IsTouching(ContactFilterLeft);

    void Start()
    {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody2D>();    
        Time.timeScale = 1;    
    }

    void Update()
    {
        Move();
        Jump();
    }

    

    void Move() // Directional Movement
    {
        
        float moveInput = Input.GetAxis("Horizontal");

        if ((moveInput < 0 && !IsTouchingLeft) || (moveInput > 0 && !IsTouchingRight) )
        {
            rb.linearVelocityX = moveInput * moveSpeed;
        }
        else if ((moveInput < 0 && IsTouchingLeft) || (moveInput > 0 && IsTouchingRight) )
        {
            rb.linearVelocityX = 0;
        }

        
    }

    void Jump() // Vertical Movement
    {
        if (Input.GetButtonDown("Jump") && IsGrounded)
        {
            rb.linearVelocityY = jumpHeight;   
        }
    }

    void OnTriggerEnter2D(Collider2D collision)  // Spikes - death, Goal - win
    {
        switch( collision.gameObject.tag.ToString() )
        {
            case "Spikes":
                onLevelFail?.Invoke();
                break;

            case "Goal":
                onLevelComplete?.Invoke(SceneManager.GetActiveScene().name, GameManager.Instance.getTimeSinceLoad() );
                break;
        }

        
    }

}
