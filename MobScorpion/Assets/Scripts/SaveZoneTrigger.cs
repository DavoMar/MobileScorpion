using UnityEngine;

public class SaveZoneTrigger : MonoBehaviour
{
    public static bool canSave = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered save zone. Saving is now allowed.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            canSave = true;
            Debug.Log("Player exited save zone.");
        }
    }
}