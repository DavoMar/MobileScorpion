using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveUp : MonoBehaviour
{
    public Transform player1;
    public float targetY;
    public float moveSpeed = 2f;
    public float minX; // Left boundary
    public float maxX; // Right boundary

    private PlayerMovement playerMovement1;
    private bool isMoving = true;
    private bool followPlayer = false;

    private void Start()
    {
        playerMovement1 = player1.GetComponent<PlayerMovement>();

        if (playerMovement1 != null) playerMovement1.enabled = false;
    }

    private void Update()
    {
        if (isMoving)
        {
            MoveCameraUp();
        }
        else if (followPlayer)
        {
            FollowPlayer();
        }
    }

    private void MoveCameraUp()
    {
        Vector3 targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Mathf.Abs(transform.position.y - targetY) < 0.01f)
        {
            isMoving = false;
            followPlayer = true;
            if (playerMovement1 != null) playerMovement1.enabled = true;
        }
    }

    private void FollowPlayer()
    {
        float clampedX = Mathf.Clamp(player1.position.x, minX, maxX);
        Vector3 targetPosition = new Vector3(clampedX, player1.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
