using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public int playerID;
    public float moveSpeed = 5f;
    public bool isAttacking = false;
    public Animator animator;

    private Rigidbody2D rb;
    [HideInInspector]
    public Vector2 lastFacingDirection = Vector2.right; 
    private SpriteRenderer rend;

    public int arrowDamage = 2;

    public Joystick joystick; // Reference to the UI Joystick

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        MovePlayer();
    }
    
void MovePlayer()
{
    // Get keyboard input
    float moveX = Input.GetAxisRaw("Horizontal");
    float moveY = Input.GetAxisRaw("Vertical");

    // Get joystick input
    float joystickX = joystick.Horizontal;
    float joystickY = joystick.Vertical;

    // Prioritize joystick input if it is active
    Vector2 movement;
    if (Mathf.Abs(joystickX) > 0.1f || Mathf.Abs(joystickY) > 0.1f)
    {
        movement = new Vector2(joystickX, joystickY);
    }
    else
    {
        movement = new Vector2(moveX, moveY);
    }

    if (movement.magnitude > 0.1f) // Small threshold to prevent micro-movements
    {
        movement.Normalize();
        rb.velocity = movement * moveSpeed;

        if (!isAttacking)
            lastFacingDirection = movement;

        HandleAnimations(movement);

        if (movement.x != 0)
        {
            rend.flipX = movement.x > 0;
        }
    }
    else
    {
        rb.velocity = Vector2.zero;
        HandleIdleAnimations();
    }
}


    void HandleAnimations(Vector2 movement)
{
    animator.SetBool("moveUp", false);
    animator.SetBool("moveDown", false);
    animator.SetBool("moveSide", false);
    animator.SetBool("moveUpRight", false);
    animator.SetBool("moveUpLeft", false);
    animator.SetFloat("moveSpeed", 1);

    float diagonalThreshold = 0.5f; // Adjust this value if needed

    if (Mathf.Abs(movement.y) > diagonalThreshold * Mathf.Abs(movement.x))
    {
        if (movement.y > 0 && movement.x > 0)
        {
            animator.SetBool("moveUpRight", true);
        }
        else if (movement.y > 0 && movement.x < 0)
        {
            animator.SetBool("moveUpLeft", true);
        }
        else if (movement.y > 0)
        {
            animator.SetBool("moveUp", true);
        }
        else if (movement.y < 0)
        {
            animator.SetBool("moveDown", true);
        }
    }
    else
    {
        if (movement.x != 0)
        {
            animator.SetBool("moveSide", true);
        }
    }
}


    void HandleIdleAnimations()
    {
        animator.SetBool("moveUp", false);
        animator.SetBool("moveDown", false);
        animator.SetBool("moveSide", false);
        animator.SetBool("moveUpRight", false);
        animator.SetBool("moveUpLeft", false);
        animator.SetFloat("moveSpeed", 0);
    }

    public void ApplyPermanentSpeedBoost(float speedMultiplier)
    {
        moveSpeed *= speedMultiplier;
    }

    public int GetArrowDamage()
    {
        return arrowDamage;
    }

    public void IncreaseArrowDamage(int amount)
    {
        arrowDamage += amount;
    }
}
