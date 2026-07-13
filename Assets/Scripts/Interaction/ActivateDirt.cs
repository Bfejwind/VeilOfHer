using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

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

    [SerializeField] 
    public float fadeAmount = 0.2f; // Fixed amount to reduce opacity

    [SerializeField] 
    public LayerMask decalLayer; // Layer mask to filter raycast hits

     public float interactionRange = 3f;

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
            PerformDecalRaycast();
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
        // decalProjector.fadeFactor = Mathf.Sin(Time.time) * 0.5f + 0.5f;
        // dirt.SetActive(false);
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

    void PerformDecalRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform the raycast restricted to the decal layer
        if (Physics.Raycast(ray, out hit, interactionRange, decalLayer))
        {
            // Try to find the HDRP Decal Projector component on the hit object
            DecalProjector projector = hit.collider.GetComponent<DecalProjector>();

            if (projector != null)
            {
                // Subtract the fixed number and clamp it between 0 (invisible) and 1 (fully opaque)
                projector.fadeFactor = Mathf.Clamp01(projector.fadeFactor - fadeAmount);
                
                Debug.Log($"Decal Hit! New opacity: {projector.fadeFactor}");
                StartCoroutine(cleanUpDirt());
            }
        }
    }
}
