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
    private SpriteRenderer sp;
    public ContactFilter2D ContactFilterDown;
    public ContactFilter2D ContactFilterLeft;
    public ContactFilter2D ContactFilterRight;
    public Sprite facingLeft;
    public Sprite facingRight;
    public Sprite facingForward;


    public static event Action<String> onLevelComplete;
    public static event Action onLevelFail;
    float timeSinceLoad;
    public bool IsGrounded => rb.IsTouching(ContactFilterDown);
    public bool IsTouchingRight => rb.IsTouching(ContactFilterRight);
    public bool IsTouchingLeft => rb.IsTouching(ContactFilterLeft);

    void Start()
    {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody2D>();    
        sp = GetComponent<SpriteRenderer>();
        Time.timeScale = 1;    
    }

    void Update()
    {
        Move();
        Jump();
    }

    

    void Move() // Directional Movement
    {
        if (GameManager.Instance.IsGameOver() ) return;
        float moveInput = Input.GetAxis("Horizontal");

        if (    ( (moveInput < 0 && !IsTouchingLeft) || (moveInput > 0 && !IsTouchingRight) ) && Time.timeScale >0 )
        {
            rb.linearVelocityX = moveInput * moveSpeed;
            if (moveInput < 0)
            {
                sp.sprite = facingLeft;
            }
            else
            {
                sp.sprite = facingRight;
            }
            if (IsGrounded)
            {
                AudioManager.Instance.PlayWalkingSoundEffect();                
            }
            return;
        }
        else if ((moveInput < 0 && IsTouchingLeft) || (moveInput > 0 && IsTouchingRight) )
        {
            rb.linearVelocityX = 0;
        }

        sp.sprite = facingForward;

        
    }

    void Jump() // Vertical Movement
    {
        if (GameManager.Instance.IsGameOver() ) return;
        if (Input.GetButtonDown("Jump") && IsGrounded && Time.timeScale > 0)
        {
            rb.linearVelocityY = jumpHeight; 
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.jumpSound);  
        }
    }

    void OnTriggerEnter2D(Collider2D collision)  // Spikes - death, Goal - win
    {
        switch( collision.gameObject.tag.ToString() )
        {
            case "Spikes":
                AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.spikesSound);
                onLevelFail?.Invoke();  
                break;

            case "Goal":
                AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.goalSound);

                onLevelComplete?.Invoke(SceneManager.GetActiveScene().name );
                break;
        }

        
    }

}
