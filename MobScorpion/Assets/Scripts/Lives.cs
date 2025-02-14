using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lives : MonoBehaviour
{   
    public int maxLives = 4;
    public int currentLives;
    public List<GameObject> hearts;
    public ScaryText scaryText;

    // Start is called before the first frame update
    void Start()
    {   
       
        currentLives = maxLives;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    

    public bool isEmpty(){
        if(currentLives <= 0){
            return true;
        }
        else 
        return false;
    }

    public void reduceLives(){
        if(currentLives > 0){
            currentLives -= 1;
            Destroy (hearts[0]);
            hearts.RemoveAt(0);
        }
        
    }

    public void increaseLives(){
        if((currentLives + 1) > maxLives){
            currentLives = maxLives;
        }
        else
        currentLives += 1;
    }

    public void increaseTomax(){
        currentLives = maxLives;
    }

    public void increaseMaxLives(int z){
        if(currentLives == maxLives){
            maxLives += z;
            currentLives += z;
        }
        else 
        maxLives += z;
    }

    public void restartGame(){
        if (scaryText.isNotFinalFight)
    SceneManager.LoadScene("Main");
}
}
