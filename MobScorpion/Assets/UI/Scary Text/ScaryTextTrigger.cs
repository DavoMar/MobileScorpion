using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaryTextTrigger : MonoBehaviour
{
    [SerializeField] private float enteringTextYPos;
    [SerializeField] private float treasureRoomYPos;
    [SerializeField] private Camera cam;
    [SerializeField] private ScaryText scaryText;

    private bool enterTextDone = false;
    private bool treasureRoomTextDone = false;

    void Update()
    {
        if (cam.transform.position.y >= enteringTextYPos && !enterTextDone)
        {
            scaryText.PlayEnteringSequence ();
            enterTextDone = true;
        }
        else if (cam.transform.position.y >= treasureRoomYPos && !treasureRoomTextDone)
        {
            scaryText.PlayTreasureRoomSequence ();
            treasureRoomTextDone = true;
        }
    }
}
