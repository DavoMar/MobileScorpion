using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowEnemy : NewEnemy
{

    [SerializeField] private int damageAmount = 2;
    [SerializeField] private float retreatCooldown = 2.0f; // the duration for which the enemy retreats after attacking
    [SerializeField] private float minStrafeDist = 5f; // the distance at which the enemy attempts to strafe
    [SerializeField] private float maxStrafeDist = 6f;
    [SerializeField] private float dashSpeed = 2f;
    [SerializeField] private float thresholdAngleToShoot = 3f;
 
    private bool isStrafing = true;
    private float strafeDir;
    private float strafeDist;
    private float oldMaxSpeed;

    public override float ShapeWeight (string tag, Vector2 dir, Vector2 dirToObject, float distToObject)
    {
        if (currTarget && tag == currTarget.tag) // either Player1 or Player2
        {
            if (timeOfLastAttack + retreatCooldown < Time.time)
            {
                // move in to attack
                if (isStrafing)
                {
                    isStrafing = false;
                    strafeDir = Random.Range (0, 2) == 0 ? -1 : 1;
                    oldMaxSpeed = maxSpeed;
                    maxSpeed = dashSpeed;
                }
                float signedAngle = Vector2.SignedAngle (dir, dirToObject) / 180f * Mathf.PI; // in radians
                return Mathf.Abs (Mathf.Cos ((signedAngle + strafeDir * Mathf.PI / 4f) / 2f));
            }
            else
            {
                // retreat
                if (!isStrafing)
                {
                    isStrafing = true;
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
        if (Vector2.Angle (dir, targetDir) < thresholdAngleToShoot)
        {
            GameObject arrow = Instantiate(attackPrefab, (Vector2)transform.position + (Vector2)dir * 0.5f + Vector2.up * 0.25f, Quaternion.identity);

            Arrow arrowComponent = arrow.GetComponent<Arrow>();
            arrowComponent.SetDirection(dir);

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            arrowComponent.IncreaseDamage(damageAmount);

            return true;
        }

        return false;
    }

    public override void Move (Vector3 dir)
    {
        rb.velocity = dir * speed;
        // update animation
        anim.SetFloat ("Speed", Mathf.Clamp (speed, 0f, 1f));
        int facingDir = VectorToIndex (targetDir) / 2 + 1;
        if (timeOfLastAnimChange + 0.3f < Time.time && facingDir != anim.GetInteger ("Direction"))
        {
            anim.SetInteger ("Direction", facingDir);
            timeOfLastAnimChange = Time.time;
        }
    }
}
