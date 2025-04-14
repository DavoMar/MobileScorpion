using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private bool isEnemy = false;  // Set this to true only for enemies

    public int currHealth;
    private int lastHitByPlayerID;
    private bool immune = false;
    public GameObject coinPrefab;
    public Lives totalLives;
    private float immuneTime = 3.0f;
    public Camera mainCamera;
    private Vector3 respawnOffset = new Vector3(-5f, 0f, 0f);
    public SpriteRenderer spriteRenderer;
    public float opacity = 0.6f;
    public CircleCollider2D collider;
    public Slider healthSlider;
    private Queue<Vector3> positionHistory = new Queue<Vector3>();
    private float positionRecordInterval = 0.5f;

    void Start()
    {
        currHealth = maxHealth;
        mainCamera = Camera.main;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currHealth;
        }
        StartCoroutine(RecordPosition());
    }

    private IEnumerator RecordPosition()
    {
        while (true)
        {
            if (positionHistory.Count >= 4) // Store last 2 seconds (4 records at 0.5s interval)
            {
                positionHistory.Dequeue();
            }
            positionHistory.Enqueue(transform.position);
            yield return new WaitForSeconds(positionRecordInterval);
        }
    }

    public int GetHealth()
    {
        return currHealth;
    }

    public bool TakeDamage(int amt, int playerID)
    {
        if(immune){
            return true;
        }
        currHealth -= amt;
        lastHitByPlayerID = playerID;

        if (healthSlider != null)
        {
            healthSlider.value = currHealth;
        }

        if (currHealth <= 0)
        {     
            HandleDeath();
            return false;
        }
        return true;
    }

    private IEnumerator ImmunityTimer()
    {
        yield return new WaitForSeconds(immuneTime);
        immune = false;
        collider.enabled = true;
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }

    public void Disable()
    {
        immune = true;
        collider.enabled = false;
        spriteRenderer.color = new Color(1f, 1f, 1f, opacity);
        currHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.value = currHealth;
        }
        StartCoroutine(ImmunityTimer());
    }

    public bool TakeDamage(int amt, List<string> targets)
    {
        if (targets.Contains(gameObject.tag))
            return TakeDamage(amt, lastHitByPlayerID);

        int halvedAmt = amt / 2;
        return TakeDamage(halvedAmt * 2 < amt ? halvedAmt + 1 : halvedAmt, lastHitByPlayerID);
    }

    public void HealDamage(int amt)
    {
        currHealth += amt;
        if (currHealth > maxHealth)
            currHealth = maxHealth;
        
        if (healthSlider != null)
        {
            healthSlider.value = currHealth;
        }
    }

    public void HandleDeath()
    {
        if (isEnemy && lastHitByPlayerID != 0)
        {
            GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            coin.GetComponent<Coin>().Initialize(lastHitByPlayerID);
            Destroy(gameObject);
        }
        else if (!isEnemy)
        {
            totalLives.reduceLives(); // Reduce player lives when health reaches zero
            if (totalLives.isEmpty())
            {
                totalLives.restartGame();
            }
            else
            {
                Respawn();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Respawn()
    {
        if (positionHistory.Count > 0)
        {
            Vector3 respawnPosition = positionHistory.Peek(); // Get position 2 seconds before death
            transform.position = respawnPosition;
        }
        currHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.value = currHealth;
        }
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float duration = 2f;
        float elapsedTime = 0f;
        Color color = spriteRenderer.color;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
    }

    public void ZeroHealth()
    {
        currHealth = 0;
        if (healthSlider != null)
        {
            healthSlider.value = currHealth;
        }
    }
}
