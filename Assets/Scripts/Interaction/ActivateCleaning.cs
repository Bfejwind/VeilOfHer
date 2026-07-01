using UnityEngine;
using System.Collections;
using TMPro;

public class ActivateCleaning : MonoBehaviour, IInteractable
{
    [SerializeField]
    GameObject mop;

    [SerializeField]
    GameObject mopInventory;

    [SerializeField]
    GameObject interactionPromt;

    public static bool mopTaken = false;


    public void Interact()
    {
        if (mopTaken)
        {
            return;
        }
        else
        {
            mopTaken = true;
            mop.SetActive(false);
            interactionPromt.SetActive(false);
            mopInventory.SetActive(true);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetDescription()
    {
        return "Pick up the mop";
    }
}
