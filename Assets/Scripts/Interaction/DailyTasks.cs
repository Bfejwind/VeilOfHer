using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Events;

public class DailyTasks : MonoBehaviour
{
    [SerializeField]
    TMP_Text dirtCountText;

    [SerializeField]
    public int dirtCleaned = 0;
    public int totalDirtCleaned = 0;

    [SerializeField]
    public int collectedMemento = 0;

    [SerializeField]
    TMP_Text mementoText;

    [Header("Dirt Events")]
    [SerializeField] private UnityEvent CompletedDirtObjective;

    public static DailyTasks Instance { get; private set; }
    

    public void Start()
    {
        mementoText.text = "Mementos Collected: " + collectedMemento.ToString() + "/4";
    }
    
    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        if (dirtCleaned >= 6)
        {
            CompletedDirt();
        }
    }

    public void CompletedDirt()
    {
        CompletedDirtObjective?.Invoke();
        Debug.Log("All dirt has been cleaned!");
    }

    public void UpdateDirtCount(int amount)
    {
        dirtCleaned += amount;
        dirtCountText.text = "Complete Daily Tasks: \n\n 1. Clean the solar panels " + dirtCleaned.ToString() + "/6 \n\n 2. Water the plants 0/1";
        Debug.Log("Dirt cleaned! Total cleaned: " + dirtCleaned.ToString());
    }

    public void ResetDirtCount()
    {
        dirtCleaned = 0;
        dirtCountText.text = "Complete Daily Tasks: \n\n 1. Clean the solar panels " + dirtCleaned.ToString() + "/6 \n\n 2. Water the plants 0/1";
    }

    public void CollectedMemento()
    {
        collectedMemento += 1; 
        mementoText.text = "Memento Collected: " + collectedMemento.ToString() + "/4";
    }
}