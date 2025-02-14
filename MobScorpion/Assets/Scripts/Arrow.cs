using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Moves forward until it hits something, doing damage if it hits something with health, and reversing direction if it hits a shield
public class Arrow : MonoBehaviour
{
    public float speed = 10f;               // Speed of the arrow
    public float destroyAfter = 5f;         // Time before the arrow disappears if it doesn't hit anything
    public int damageAmount = 2;             // Base damage amount
    public List<string> validTargets = new List<string> { "Enemy" }; // Tags for valid targets

    private Rigidbody2D rb;
    private Vector2 direction;               // Direction of the arrow
    public int playerID;                     // ID of the player that shot the arrow

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;     // Set the initial movement of the arrow
        Destroy(gameObject, destroyAfter);   // Destroy after a set time if no collision occurs
    }

    // Set the direction of the arrow
    public void SetDirection(Vector2 shootDirection)
    {
        direction = shootDirection.normalized; // Set and normalize the direction
    }

    // Method to increase the damage amount
    public void IncreaseDamage(int amount)
    {
        damageAmount += amount; // Increase the damage by the specified amount
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;

        if (other.CompareTag("Shield"))
        {
            // Reverse the arrow's direction by flipping its velocity
            rb.velocity = -rb.velocity;

            // Flip the sprite horizontally
            Vector3 flippedScale = transform.localScale;
            flippedScale.x *= -1;
            transform.localScale = flippedScale;
        }
        else
        {
            Health targetHealth = other.GetComponent<Health>();
            if (targetHealth != null)
            {
                // Pass the playerID when dealing damage
                targetHealth.TakeDamage(damageAmount, playerID);
            }
            Destroy(gameObject);
        }
    }
}
