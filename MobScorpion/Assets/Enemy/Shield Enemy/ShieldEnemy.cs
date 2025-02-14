using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : NewEnemy
{
    [SerializeField] private Vector2 shieldOffset = new Vector2(0.5f, 0f);

    [SerializeField] private float retreatCooldown = 2.0f; // the duration for which the enemy retreats after attacking
    [SerializeField] private float minStrafeDist = 2f; // the distance at which the enemy attempts to strafe
    [SerializeField] private float maxStrafeDist = 4f;
    [SerializeField] private float minStopDist = 2f; // the distance at which the enemy stops in front of the player
    [SerializeField] private float maxStopDist = 1f;
 
    private bool isStrafing = true;
    private float strafeDir;
    private float strafeDist;
    private float stopDist;
    private GameObject shieldInstance;
    private Vector3 shieldDir;

    public override float ShapeWeight (string tag, Vector2 dir, Vector2 dirToObject, float distToObject)
    {
        if (currTarget && tag == currTarget.tag) // either Player1 or Player2
        {
            if (timeOfLastAttack + retreatCooldown < Time.time)
            {
                // move in to shield
                if (isStrafing)
                {
                    isStrafing = false;
                    stopDist = Random.Range (minStopDist, maxStopDist);
                }
                float angle = Vector2.Angle (dir, dirToObject) / 180f * Mathf.PI; // in radians
                float retreatWeight = Mathf.Cos (angle);
                return retreatWeight * (distToObject - stopDist) / 4f;
            }
            else
            {
                // retreat
                if (!isStrafing)
                {
                    isStrafing = true;
                    strafeDir = Random.Range (0, 2) == 0 ? -1 : 1;
                    strafeDist = Random.Range (minStrafeDist, maxStrafeDist);
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
        if (shieldInstance != null && dir != shieldDir)
        {
            // the enemy wants to rotate its shield, but it needs to lower it first
            Destroy (shieldInstance);
            return true;
        }
        else if (shieldInstance == null)
        {
            // raise the shield
            shieldDir = dir;
            shieldInstance = Instantiate(attackPrefab, transform.position + new Vector3 (0f, 0.5f, 0f), Quaternion.identity);
            shieldInstance.transform.parent = transform;
            shieldInstance.GetComponent<Shield> ().wielder = gameObject;
            return true;
        }

        // the enemy is already shielding
        return false;
    }
}
