using UnityEngine;

public class JoystickFollowCamera : MonoBehaviour
{
    public Camera mainCamera;
    public Vector3 offset = new Vector3(-300, -300, 0); // Adjust to place it in bottom-left corner

    void Update()
    {
        if (mainCamera != null)
        {
            Vector3 screenPos = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.nearClipPlane));
            transform.position = screenPos + offset;
        }
    }
}
