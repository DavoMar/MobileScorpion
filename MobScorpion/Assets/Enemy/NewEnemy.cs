using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemy : MonoBehaviour
{

    [SerializeField] protected float maxSpeed = 1f; // in units per second
    [SerializeField] protected GameObject attackPrefab;
    [SerializeField] private float range = 5f;
    [SerializeField] private float attackCooldown = 1f; // in seconds
    public List<GameObject> targets = new List<GameObject> ();
    [SerializeField] protected float leftWall = -10f; // the left & right edges of the path the enemy is on
    [SerializeField] protected float rightWall = 10f; // ground enemies should count the lava as a wall
    public Camera cam;

    private float retargetCooldown = 0.5f;
    protected Rigidbody2D rb;
    private NewEnemyVision vision;
    private List<float> weights; // how much the enemy wants to move in each of the 8 directions
    private float choiceThreshold = 0.0f;
    private float rangeInCamera; // how close to the camera the enemy must be to attack

    private float targetDist = float.PositiveInfinity;
    protected Vector2 targetDir;
    protected float timeOfLastAttack = float.NegativeInfinity;
    protected GameObject currTarget; // null if no current target
    protected int directionChoice = 0;
    private float timeOfLastRetarget = 0f;
    protected float speed = 0f;
    protected Animator anim;
    protected float timeOfLastAnimChange = float.NegativeInfinity;

    public bool drawWeights = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D> ();
        vision = GetComponentInChildren<NewEnemyVision> ();
        weights = new List<float> ();
        for (int i = 0; i < 8; ++i)
        {
            weights.Add (0f);
        }

        rangeInCamera = -cam.transform.position.z / 5f * 6f - 1f; // depends on the camera's z pos

        anim = GetComponent<Animator> ();
    }

    void Update()
    {
        // retarget
        if (timeOfLastRetarget + retargetCooldown < Time.time)
        {
            Retarget ();
            timeOfLastRetarget = Time.time;
        }

        // choose a direction
        UpdateWeights ();
        int bestChoice = IndexOfMaxWeight (weights);
        if (bestChoice == -1)
            directionChoice = -1;
        else if (directionChoice == -1)
            directionChoice = bestChoice;
        else if (weights[bestChoice] > weights[directionChoice] + choiceThreshold)
            directionChoice = bestChoice;

        if (directionChoice != -1)
        {
            // move
            if (weights[directionChoice] < 1f)
                speed = weights[directionChoice] * maxSpeed;
            else
                speed = maxSpeed;
            Vector2 dir = IndexToVector (directionChoice);
            Move (dir);

            // attack the target
            if (currTarget != null)
            {
                targetDist = Mathf.Abs((currTarget.transform.position - transform.position).magnitude);
                targetDir = (currTarget.transform.position - transform.position).normalized;
                if (timeOfLastAttack + attackCooldown < Time.time && targetDist < range && transform.position.y - cam.transform.position.y < rangeInCamera)
                {
                    if (Attack (IndexToVector (VectorToIndex (targetDir))))
                        timeOfLastAttack = Time.time;
                }
            }
        }
        else
        {
            Move (new Vector3 (0f, 0f, 0f));
        }

        if (drawWeights)
                DrawDebugWeights ();

        if (transform.position.y - cam.transform.position.y < -rangeInCamera * 2)
            Destroy (gameObject);
    }

    // choose a new target based on proximity
    public void Retarget ()
    {
        if (!targets.Contains (currTarget))
        {
            currTarget = null;
            targetDist = float.PositiveInfinity;
        }
        foreach (GameObject t in targets)
        {
            if (t != null)
            {
                float tDist = Mathf.Abs((t.transform.position - transform.position).magnitude);
                if (tDist < targetDist)
                {
                    currTarget = t;
                    targetDist = tDist;
                }
            }
        }
    }

    // decide which directions are best to move in based on nearby entities
    private void UpdateWeights ()
    {
        weights = new List<float> {0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
        foreach (GameObject g in vision.objects)
        {
            // update each weight
            for (int i = 0; i < 8; ++i)
            {
                Vector2 dir = IndexToVector (i);
                Vector2 dirToObject = (Vector2) (g.transform.position - transform.position).normalized;
                float dist = Vector3.Distance (g.transform.position, transform.position);
                weights[i] += ShapeWeight (g.tag, dir, dirToObject, dist);
            }
        }

        // adjust each weight based on proximity to the left & right walls
        float distToLeft = transform.position.x - leftWall - 0.25f; // account for the size of the enemy's circular hitbox
        if (distToLeft < 0f)
            distToLeft = 0f;
        float distToRight = rightWall - transform.position.x - 0.25f; // account for the size of the enemy's circular hitbox
        if (distToRight < 0f)
            distToRight = 0f;
        for (int i = 0; i < 8; ++i)
        {
            Vector2 dir = IndexToVector (i);

            // left wall 
            float dot = Vector2.Dot (dir, Vector2.left);
            if (dot < 0f)
                dot = 0f;
            weights[i] -= 0.05f * dot / distToLeft / distToLeft / distToLeft;

            // right wall
            dot = Vector2.Dot (dir, Vector2.right);
            if (dot < 0f)
                dot = 0f;
            weights[i] -= 0.05f * dot / distToRight / distToRight / distToRight;
        }

        // TODO: avoid falling in lava from behind
    }

    // apply a shaping function to a weight, based on the object's tag
    public virtual float ShapeWeight (string tag, Vector2 dir, Vector2 dirToObject, float distToObject)
    {
        if (currTarget && tag == currTarget.tag) // either Player1 or Player2
        {
            float angle = Vector2.Angle (dir, dirToObject) / 180f * Mathf.PI; // in radians
            return Mathf.Cos (angle / 2f); // / distToObject;
        }
        else if (tag == "Wall Entity" || tag == "Enemy")
        {
            distToObject -= 1f;
            if (distToObject < 0f) distToObject = 0f;
            float dot = Vector2.Dot (dir, dirToObject);
            if (dot < 0f)
                dot = 0f;
            return -1f * dot / distToObject / distToObject / distToObject;
        }
        return 0f;
    }

    // advance in a certain direction
    public virtual void Move (Vector3 dir)
    {
        rb.velocity = dir * speed;
    }

    // attack in a certain direction
    // returns true if the enemy did indeed attack (some subclasses might add further logic)
    public virtual bool Attack (Vector3 dir)
    {
        // Instantiate the arrow prefab
        GameObject arrow = Instantiate(attackPrefab, transform.position + dir, Quaternion.identity);

        // Get the Arrow component and set its direction
        Arrow arrowComponent = arrow.GetComponent<Arrow>();
        arrowComponent.SetDirection(dir);

        // Rotate the arrow to match the shooting direction
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        return true;
    }

    private int IndexOfMaxWeight (List<float> weights)
    {
        float maxVal = 0f;
        int index = -1;
        for (int i = 0; i < 8; ++i)
        {
            if (weights[i] > maxVal)
            {
                maxVal = weights[i];
                index = i;
            }
        }
        return index;
    }

    // index 0 is Vector2.up
    public Vector2 IndexToVector (int i)
    {
        float angle = ((float) i) / 8f * 2f * Mathf.PI; // in radians
        return new Vector2 (Mathf.Sin (angle), Mathf.Cos (angle));
    }

    // nearest of the 8 directions to a given Vector2
    public int VectorToIndex (Vector2 dir)
    {
        float angle = Vector2.SignedAngle (dir, Vector2.down) + 180f; // in degrees
        int i = (int) Mathf.Round (angle / 45f);
        return i == 8 ? 0 : i;
    }



    // from ChatGPT, for debug
    void DrawDebugWeights()
    {
        if (weights == null || weights.Count != 8) return;

        // Set the base position of the enemy
        Vector3 position = transform.position;

        // Iterate through each direction (8 directions total)
        for (int i = 0; i < 8; i++)
        {
            // Get the direction vector for this index
            Vector2 direction = IndexToVector(i);

            // Get the weight and determine the color
            float weight = weights[i];
            Color lineColor = weight >= 0 ? Color.green : Color.red;

            // Calculate the endpoint of the line, with length proportional to the absolute weight
            Vector3 endPoint = position + (Vector3)direction * Mathf.Log(1f + Mathf.Abs(weight));

            // Draw the line in the Game view
            Debug.DrawLine(position, endPoint, lineColor);
        }
    }

}
