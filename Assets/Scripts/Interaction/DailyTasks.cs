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
        dirtCountText.text = "Complete Daily Tasks: \n 1. Clean the solar panels " + dirtCleaned.ToString() + "/6 \n 2. Water the plants 0/1";
        Debug.Log(dirtCleaned);
    }

    public void ResetDirtCount()
    {
        dirtCleaned = 0;
        dirtCountText.text = "Complete Daily Tasks: \n 1. Clean the solar panels " + dirtCleaned.ToString() + "/6 \n 2. Water the plants 0/1";
    }

    public void CompletedTasks()
    {
        if (dirtCleaned >= 6)
        {
            objectives.dirtTaskCompleted = true;
            objectives.CompleteDirtObjective();
            Debug.Log("All tasks completed!");
        }
    }

    public void CollectedMemento()
    {
        collectedMemento += 1; 
        mementoText.text = "Memento Collected: " + collectedMemento.ToString() + "/4";
    }
}