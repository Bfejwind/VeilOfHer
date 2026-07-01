using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class ActivateDirt : MonoBehaviour, IInteractable
{
    [SerializeField]
    GameObject interactionPromt;

    [SerializeField]
    GameObject dirt;

    [SerializeField]
    TMP_Text dirtCountText;

    [SerializeField]
    public TMP_Text statusText;

    public ActivateCleaning mopStatus;

    public DailyTasks dirtCount;

    public int dirtCleanedUp = 0;

    void Start()
    {
        mopStatus = GetComponent<ActivateCleaning>();
        dirtCount = GetComponent<DailyTasks>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Mop Taken: " + ActivateCleaning.mopTaken);
        Debug.Log("Dirt Cleaned: " + DailyTasks.dirtCleaned);
    }

    public void Interact()
    {
        if (ActivateCleaning.mopTaken)
        {
            cleanDirt();
        }
        else
        {
            statusText.text = "You need a mop to clean the dirt!";
            StartCoroutine(WaitForSeconds(2));
            return;
        }
    }

    public string GetDescription()
    {
        return "Clean the dirt";
    }
    
    IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        statusText.text = "";
    }

    public void cleanDirt()
    {
        StartCoroutine(cleanUpDirt());
    }

    IEnumerator cleanUpDirt()
    {
        dirt.SetActive(false);
        if (dirtCount != null)
        {
            if (DailyTasks.dirtCleaned < 3)
            {
                dirtCount.UpdateDirtCount();
            }
        }
        else
        {
            Debug.LogWarning("DailyTasks reference is null. Cannot update dirt count.");
        }
        yield return new WaitForSeconds(2);
    }
}
