using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMoveUp : MonoBehaviour
{
    public Transform player1;
    public float targetY;
    public float moveSpeed = 2f;
    public float followSpeed = 1f; // Speed when following player
    public float minX; // Left boundary
    public float maxX; // Right boundary
    public float minY; // Y-axis limit (prevents camera from going too low)

    public float sideMargin = 1f; // Customizable margin for left/right edges

    public GameObject joystickObject; // UI Joystick (Parent)
    public float maxJoystickOpacityChild1 = 1f; // Max opacity for the first child
    public float maxJoystickOpacityChild2 = 0.5f; // Max opacity for the second child
    public float fadeSpeed = 1f; // Speed of the fade-in effect

    private PlayerMovement playerMovement1;
    private bool isMoving = true;
    private bool followPlayer = false;
    private Image[] joystickChildrenImages; // Array to store all child Image components

    private void Start()
    {
        playerMovement1 = player1.GetComponent<PlayerMovement>();

        if (playerMovement1 != null) 
            playerMovement1.enabled = false;

        if (joystickObject != null)
        {
            // Get all child Image components
            joystickChildrenImages = joystickObject.GetComponentsInChildren<Image>(true);

            // Set all child images to fully transparent at start (excluding the parent)
            foreach (Image child in joystickChildrenImages)
            {
                // Skip the parent (joystickObject) and only affect the children
                if (child.transform != joystickObject.transform)
                {
                    Color color = child.color;
                    color.a = 0f;
                    child.color = color;
                }
            }
        }
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

        RestrictPlayerMovement();
    }

    private void MoveCameraUp()
    {
        Vector3 targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Mathf.Abs(transform.position.y - targetY) < 0.01f)
        {
            isMoving = false;
            followPlayer = true;
            if (playerMovement1 != null) 
                playerMovement1.enabled = true;

            if (joystickChildrenImages != null)
                StartCoroutine(FadeInJoystickChildren()); // Start joystick fade-in
        }
    }

    private void FollowPlayer()
    {
        float clampedX = Mathf.Clamp(player1.position.x, minX, maxX);
        float clampedY = Mathf.Max(player1.position.y, minY); // Prevents camera from moving below minY

        Vector3 targetPosition = new Vector3(clampedX, clampedY, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }

    private void RestrictPlayerMovement()
    {
        float cameraHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        float cameraBottom = transform.position.y - Camera.main.orthographicSize;

        float playerMinX = transform.position.x - cameraHalfWidth + sideMargin;
        float playerMaxX = transform.position.x + cameraHalfWidth - sideMargin;
        float playerMinY = cameraBottom + 0.5f;

        float clampedPlayerX = Mathf.Clamp(player1.position.x, playerMinX, playerMaxX);
        float clampedPlayerY = Mathf.Max(player1.position.y, playerMinY);

        player1.position = new Vector3(clampedPlayerX, clampedPlayerY, player1.position.z);
    }

    private IEnumerator FadeInJoystickChildren()
    {
        bool allFaded = false;
        
        while (!allFaded)
        {
            allFaded = true; // Assume all are faded unless proven otherwise

            for (int i = 0; i < joystickChildrenImages.Length; i++)
            {
                Image child = joystickChildrenImages[i];
                if (child.transform != joystickObject.transform) // Skip parent
                {
                    Color color = child.color;

                    // Determine max opacity based on the child index
                    float maxOpacity1 = maxJoystickOpacityChild1;
                    float maxOpacity2 = maxJoystickOpacityChild2;

                    if (color.a < maxOpacity1)
                    {
                        color.a += Time.deltaTime * fadeSpeed;
                        child.color = color;
                        allFaded = false; // Still fading
                    }
                    if (color.a < maxOpacity2)
                    {
                        color.a += Time.deltaTime * fadeSpeed;
                        child.color = color;
                        allFaded = false; // Still fading
                    }
                }
            }
            yield return null;
        }

        // Ensure all elements reach their respective full opacity
        for (int i = 0; i < joystickChildrenImages.Length; i++)
        {
            Image child = joystickChildrenImages[i];
            if (child.transform != joystickObject.transform) // Skip parent
            {
                Color color = child.color;

                // Determine max opacity based on the child index
                float maxOpacity1 = maxJoystickOpacityChild1;
                float maxOpacity2 = maxJoystickOpacityChild2;

                color.a = maxOpacity1;
                color.a = maxOpacity2;
                child.color = color;
            }
        }
    }
}
