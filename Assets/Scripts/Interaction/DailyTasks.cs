using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class DailyTasks : MonoBehaviour
{
    [SerializeField]
    TMP_Text dirtCountText;

    [SerializeField]
    public static int dirtCleaned = 0;


    public void Start()
    {
        dirtCountText.text = "Dirt Cleaned: " + dirtCleaned.ToString() + "/3";
    }

    public void UpdateDirtCount()
    {
        dirtCleaned += 1;
        dirtCountText.text = "Dirt Cleaned: " + dirtCleaned.ToString() + "/3";
        Debug.Log(dirtCleaned);
    }

    public void ResetDirtCount()
    {
        dirtCleaned = 0;
        dirtCountText.text = "Dirt Cleaned: " + dirtCleaned.ToString() + "/3";
    }

    public void CompletedTasks()
    {
        if (dirtCleaned >= 3)
        {
            Debug.Log("All tasks completed!");
            // You can add additional logic here for when all tasks are completed.
        }
    }
}