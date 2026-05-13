using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Light lightRadius;
    private float lightMin;


    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        //Persist across scene changes
        DontDestroyOnLoad(gameObject);
        //Initiate values
        lightMin = 7.0f;
        //Clamp light radius
        lightRadius.range = Mathf.Clamp(lightRadius.range,7,20);

        StartCoroutine(ReduceVision(2.0f,1.0f));
    }
    private IEnumerator ReduceVision(float interval, float amount)
    {
        while (true)
        {
            if (lightRadius.range > lightMin) //7 is placeholder amount,very low visiblility
            {
                lightRadius.range -= amount;
                if (lightRadius.range < lightMin)
                {
                    lightRadius.range = lightMin;
                }
                Debug.Log("Light range: " + lightRadius.range);
                yield return new WaitForSeconds(interval);
            }
            else
            {
                yield return new WaitForSeconds(interval);
            }
        }
    }

    public void ChangeLightRadius(float value)
    {
        lightRadius.range += value;
        if (lightRadius.range > 20f)
        {
            lightRadius.range = 20f;
        }
    }
}
