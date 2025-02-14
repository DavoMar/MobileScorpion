using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// waits 0.1 seconds and then does damage to everything it hit, except for anything using a shield
public class SwordHitbox : MonoBehaviour
{
    public int DamageAmount { get; set; }
    public int playerID;  // Ensure this is assigned from SwordSwing

    private List<GameObject> enemiesToDamage = new List<GameObject>();
    private List<GameObject> enemiesNotToDamage = new List<GameObject>();
    private float timeUntilDoDamage = 0.1f;
    private float timeOfSpawn;
    private bool damageDone = false;

    void Start()
    {
        timeOfSpawn = Time.time;
    }

    void Update()
    {
        if (timeOfSpawn + timeUntilDoDamage < Time.time && !damageDone)
        {
            DoDamage();
            damageDone = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Health enemyHealth = collision.collider.GetComponent<Health>();
        GameObject other = collision.gameObject;
        if (enemyHealth != null)
        {
            enemiesToDamage.Add(other);
        }
        else if (other.CompareTag("Shield"))
        {
            enemiesNotToDamage.Add(other.GetComponent<Shield>().wielder);
        }
    }

    private void DoDamage()
    {
        foreach (GameObject g in enemiesToDamage)
        {

            if (g && !enemiesNotToDamage.Contains(g))
            {
                Health health = g.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(DamageAmount, playerID);  // Use the correct playerID here
                }
            }
        }
    }
}


