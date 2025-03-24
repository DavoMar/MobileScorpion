using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMov : MonoBehaviour
{
    public Transform player1;
    public float camSpeed = 1f;
    public float followSpeed = 1f;
    public float boundHeight = 200f;
    public float minY; // Prevents camera from going too low

    public CamTrigger startOne;
    public CamTrigger startTwo;
    public CamTrigger startThree;
    public CamTrigger startFour;
    public CamTrigger stopOne;
    public CamTrigger stopTwo;
    public CamTrigger stopThree;
    public CamTrigger stopFour;

    private bool moving = false;
    private bool followPlayer = false;
    private bool stopContact = false;

    private void Update()
    {
        CheckTriggers();

        if (followPlayer)
        {
            FollowPlayer();
        }
        else if (moving)
        {
            MoveCameraUp();
        }
    }

    private void CheckTriggers()
    {
        // Start movement triggers
        if (startOne.moveCam())
        {
            StartFollowing(110);
        }
        else if (startTwo.moveCam())
        {
            StartFollowing(230);
        }
        else if (startThree.moveCam())
        {
            StartFollowing(460);
        }
        else if (startFour.moveCam())
        {
            StartFollowing(495);
        }

        // Stop movement triggers
        if (stopOne.moveCam() || stopTwo.moveCam() || stopThree.moveCam() || stopFour.moveCam())
        {
            StopFollowing();
        }
    }

    private void StartFollowing(float newBoundHeight)
    {
        boundHeight = newBoundHeight;
        moving = true;
        followPlayer = true;
        stopContact = false;
    }

    private void StopFollowing()
    {
        moving = false;
        followPlayer = false;
        stopContact = true;
    }

    private void MoveCameraUp()
    {
        Vector3 newPosition = transform.position + Vector3.up * camSpeed * Time.deltaTime;
        if (newPosition.y <= boundHeight)
        {
            transform.position = newPosition;
        }
    }

    private void FollowPlayer()
{
    if (player1 == null)
    {
        Debug.LogWarning("Player1 reference is missing!");
        return;
    }

    float clampedY = Mathf.Max(player1.position.y, minY); // Prevents the camera from going below minY
    Vector3 targetPosition = new Vector3(transform.position.x, clampedY, transform.position.z);
    transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
}
}
