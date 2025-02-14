using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LavaLight : MonoBehaviour
{

    public float maxIntensity = 1f;
    
    private float timeOfSpawn;

    private float waitDuration = 0f;
    private float brightenDuration = 1f;
    private Light2D light2D;

    void Start()
    {
        light2D = GetComponent<Light2D> ();
        timeOfSpawn = Time.time;
    }

    void Update()
    {
        if (timeOfSpawn + waitDuration < Time.time && timeOfSpawn + waitDuration + brightenDuration > Time.time)
        {
            light2D.intensity += maxIntensity * Time.deltaTime / brightenDuration;
        }
    }
}
