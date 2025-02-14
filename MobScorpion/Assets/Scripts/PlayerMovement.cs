using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// adjusts the player's vecolity depending on keypresses
// also keeps track of whether the player is attacking or not

public class PlayerMovement : MonoBehaviour
{

    public int playerID;

    public float moveSpeed = 5f;
    public KeyCode upButton = KeyCode.W;
    public KeyCode downButton = KeyCode.S;
    public KeyCode leftButton = KeyCode.A;
    public KeyCode rightButton = KeyCode.D;
    public bool isAttacking = false;
    public Animator animator;

    private Rigidbody2D rb;
    [HideInInspector]
    public Vector2 lastFacingDirection = Vector2.right; // Default facing direction
    private SpriteRenderer rend;

    public int arrowDamage = 2;

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
        Vector2 movement = Vector2.zero;

        if (Input.GetKey(upButton)) movement.y += 1;
        if (Input.GetKey(downButton)) movement.y -= 1;
        if (Input.GetKey(leftButton)) movement.x -= 1;
        if (Input.GetKey(rightButton)) movement.x += 1;

        if (movement != Vector2.zero)
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
        else if (movement.x != 0)
        {
            animator.SetBool("moveSide", true);
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



    // Method to apply a permanent speed power-up
    public void ApplyPermanentSpeedBoost(float speedMultiplier)
    {
        moveSpeed *= speedMultiplier; // Permanently increase the speed
    }
    public int GetArrowDamage()
    {
        return arrowDamage; // Return the current arrow damage value
    }
    public void IncreaseArrowDamage(int amount)
    {
        arrowDamage += amount; // Increase the arrow damage
    }

}