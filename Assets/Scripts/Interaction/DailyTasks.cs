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

    [SerializeField]
    public int collectedMemento = 0;

    [SerializeField]
    TMP_Text mementoText;

    ObjectiveManager objectives;

    public void Start()
    {
        objectives = GetComponent<ObjectiveManager>();
    }

    public void UpdateDirtCount()
    {
        dirtCleaned += 1;
        dirtCountText.text = "Complete Daily Tasks: \n 1. Clean the dirt in the house " + dirtCleaned.ToString() + "/5 \n 2. Clean the solar panels";
        Debug.Log(dirtCleaned);
    }

    public void ResetDirtCount()
    {
        dirtCleaned = 0;
        dirtCountText.text = "Complete Daily Tasks: \n 1. Clean the dirt in the house " + dirtCleaned.ToString() + "/5 \n 2. Clean the solar panels";
    }

    public void CompletedTasks()
    {
        if (dirtCleaned >= 5)
        {
            objectives.dailyTaskCompleted = true;
            Debug.Log("All tasks completed!");
            // You can add additional logic here for when all tasks are completed.
        }
    }

    public void CollectedMemento()
    {
        collectedMemento += 1; 
        mementoText.text = "Memento Collected: " + collectedMemento.ToString() + "/4";
    }
}