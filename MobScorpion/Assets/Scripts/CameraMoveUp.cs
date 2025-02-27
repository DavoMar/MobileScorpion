using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMoveUp : MonoBehaviour
{
    public Transform player1;
    public float targetY;
    public float moveSpeed = 2f;
    public float followSpeed = 1f;
    public float minX;
    public float maxX;
    public float minY;
    public float sideMargin = 1f;

    public GameObject joystickObject;
    public float maxJoystickOpacityChild1 = 1f;
    public float maxJoystickOpacityChild2 = 0.5f;
    public float fadeSpeed = 1f;

    public GameObject[] buttonObjects; // Array for the 3 buttons
    public float maxButtonOpacity = 1f; // Opacity for buttons

    private PlayerMovement playerMovement1;
    private bool isMoving = true;
    private bool followPlayer = false;
    private Image[] joystickChildrenImages;
    private Image[] buttonImages; // Array to store Image components of the buttons

    private void Start()
    {
        playerMovement1 = player1.GetComponent<PlayerMovement>();

        if (playerMovement1 != null) 
            playerMovement1.enabled = false;

        if (joystickObject != null)
        {
            joystickChildrenImages = joystickObject.GetComponentsInChildren<Image>(true);
            foreach (Image child in joystickChildrenImages)
            {
                if (child.transform != joystickObject.transform)
                {
                    Color color = child.color;
                    color.a = 0f; // Start with 0 opacity
                    child.color = color;
                }
            }
        }

        // Store button images and initialize opacity to 0
        List<Image> tempButtonImages = new List<Image>();
        foreach (GameObject button in buttonObjects)
        {
            if (button != null)
            {
                Image buttonImage = button.GetComponentInChildren<Image>(); // Get the Image component inside the button
                if (buttonImage != null)
                {
                    Color color = buttonImage.color;
                    color.a = 0f; // Start fully transparent
                    buttonImage.color = color;
                    tempButtonImages.Add(buttonImage);
                }
            }
        }
        buttonImages = tempButtonImages.ToArray();
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
                StartCoroutine(FadeInJoystickChildren()); 

            if (buttonImages != null)
                StartCoroutine(FadeInButtonImages()); // Use FadeInButtonImages for button images
        }
    }

    private void FollowPlayer()
    {
        float clampedX = Mathf.Clamp(player1.position.x, minX, maxX);
        float clampedY = Mathf.Max(player1.position.y, minY);

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
            allFaded = true;

            for (int i = 0; i < joystickChildrenImages.Length; i++)
            {
                Image child = joystickChildrenImages[i];
                if (child.transform != joystickObject.transform)
                {
                    Color color = child.color;
                    float targetOpacity = (i == 0) ? maxJoystickOpacityChild1 : maxJoystickOpacityChild2;

                    if (color.a < targetOpacity)
                    {
                        color.a += Time.deltaTime * fadeSpeed;
                        child.color = color;
                        allFaded = false;
                    }
                }
            }
            yield return null;
        }
    }

    private IEnumerator FadeInButtonImages()
    {
        bool allFaded = false;

        while (!allFaded)
        {
            allFaded = true;

            foreach (Image buttonImage in buttonImages)
            {
                Color color = buttonImage.color;
                if (color.a < maxButtonOpacity)
                {
                    color.a += Time.deltaTime * fadeSpeed;
                    buttonImage.color = color;
                    allFaded = false;
                }
            }
            yield return null;
        }

        // Ensure all elements reach full opacity (if needed)
        foreach (Image buttonImage in buttonImages)
        {
            Color color = buttonImage.color;
            color.a = maxButtonOpacity;
            buttonImage.color = color;
        }
    }
}
