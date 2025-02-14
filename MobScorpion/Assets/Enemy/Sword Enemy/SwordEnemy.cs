using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEnemy : NewEnemy
{

    [SerializeField] private int damageAmount = 5;
    [SerializeField] private float swingDuration = 0.2f;
    [SerializeField] private Vector2 hitboxOffset = new Vector2(1f, 0f);

    [SerializeField] private float retreatCooldown = 2.0f; // the duration for which the enemy retreats after attacking
    [SerializeField] private float minStrafeDist = 2f; // the distance at which the enemy attempts to strafe
    [SerializeField] private float maxStrafeDist = 4f;
    [SerializeField] private float dashSpeed = 2f;
 
    private float strafeDir = 1f;
    private float strafeDist;
    private float oldMaxSpeed;

    public override float ShapeWeight (string tag, Vector2 dir, Vector2 dirToObject, float distToObject)
    {
        if (currTarget && tag == currTarget.tag) // either Player1 or Player2
        {
            if (timeOfLastAttack + retreatCooldown < Time.time)
            {
                // move in to attack
                if (strafeDir != 0f)
                {
                    strafeDir = 0f;
                    oldMaxSpeed = maxSpeed;
                    maxSpeed = dashSpeed;
                }
                float angle = Vector2.Angle (dir, dirToObject) / 180f * Mathf.PI; // in radians
                return Mathf.Cos (angle / 2f);
            }
            else
            {
                // retreat
                if (strafeDir == 0f)
                {
                    strafeDir = Random.Range (0, 2) == 0 ? -1 : 1;
                    strafeDist = Random.Range (minStrafeDist, maxStrafeDist);
                    maxSpeed = oldMaxSpeed;
                }
                
                float angle = Vector2.Angle (dir, dirToObject) / 180f * Mathf.PI; // in radians
                float retreatWeight = Mathf.Cos (angle);
                float strafeWeight;
                float signedAngle = Vector2.SignedAngle (dir, dirToObject) / 180f * Mathf.PI; // in radians
                if (strafeDir < 1)
                    strafeWeight = Mathf.Abs (Mathf.Cos ((signedAngle + Mathf.PI / 2f) / 2f));
                else
                    strafeWeight = Mathf.Abs (Mathf.Cos ((signedAngle - Mathf.PI / 2f) / 2f));

                return strafeWeight + retreatWeight * (distToObject - strafeDist) / 4f;
            }
        }
        else if (tag == "Wall Entity")
        {
            distToObject -= 1f;
            if (distToObject < 0f)
                distToObject = 0f;

            float dot = Vector2.Dot (dir, dirToObject);
            if (dot < 0f)
                dot = 0f;
            return -1f * dot / distToObject / distToObject / distToObject;
        }
        else if (tag == "Enemy")
        {
            distToObject -= 0.5f;
            if (distToObject < 0f)
                distToObject = 0f;

            float dot = Vector2.Dot (dir, dirToObject);
            if (dot < 0f)
                dot = 0f;
            return -0.05f * dot / distToObject / distToObject / distToObject;
        }
        return 0f;
    }

    public override bool Attack (Vector3 dir)
    {
        StartCoroutine(SwingSword((Vector2) dir));
        return true;
    }

    IEnumerator SwingSword(Vector2 dir)
    {        
        Vector2 spawnPosition = (Vector2)transform.position + dir * hitboxOffset.magnitude + new Vector2(0f, 0.25f);
        GameObject hitbox = Instantiate(attackPrefab, spawnPosition, Quaternion.identity, transform);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        hitbox.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Apply damage to any targets in the hitbox
        SwordHitbox swordHitbox = hitbox.GetComponent<SwordHitbox>();
        if (swordHitbox != null)
        {
            swordHitbox.DamageAmount = damageAmount;
        }

        // Ensure the hitbox has a Collider2D component set for collision
        Collider2D collider = hitbox.GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.isTrigger = false; // Ensure it's set for collision, not trigger
        }

        // Wait for the swing duration
        yield return new WaitForSeconds(swingDuration);

        // Destroy the hitbox
        Destroy(hitbox);
    }

    public override void Move (Vector3 dir)
    {
        rb.velocity = dir * speed;
        // update animation
        anim.SetFloat ("Speed", Mathf.Clamp (speed, 0f, 1f));
        int facingDir = directionChoice / 2 + 1;
        if (timeOfLastAnimChange + 0.3f < Time.time && facingDir != anim.GetInteger ("Direction"))
        {
            anim.SetInteger ("Direction", facingDir);
            timeOfLastAnimChange = Time.time;
        }
    }
}
