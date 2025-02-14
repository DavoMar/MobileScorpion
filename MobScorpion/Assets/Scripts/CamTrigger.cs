using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTrigger : MonoBehaviour
{
    private bool inContact;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool moveCam(){
        return inContact;
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.CompareTag("Player1")){
            inContact = true;
        }

        if (collision.gameObject.CompareTag("Player2")){
            inContact = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
        if (collision.gameObject.CompareTag("Player1")){
            inContact = false;
        }

        if (collision.gameObject.CompareTag("Player2")){
            inContact = false;
        }
    }
}
