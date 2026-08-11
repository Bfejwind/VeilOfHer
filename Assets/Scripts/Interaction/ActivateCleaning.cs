using UnityEngine;
using System.Collections;
using TMPro;

public class ActivateCleaning : MonoBehaviour, IInteractable
{
    [Header("Cleaning Objects")]
    [Tooltip("The mop object that the player can pick up.")]
    [SerializeField]
    public GameObject mop;

    [Header("Inventory Objects")]
    [Tooltip("The inventory object that represents the mop in the player's inventory.")]
    [SerializeField]
    public GameObject mopInventory;

    [Header("Interaction UI")]
    [Tooltip("The UI element that prompts the player to interact with the mop.")]
    [SerializeField]
    public GameObject interactionPromt;

    [Header("Mop Placement")]
    [Tooltip("The location where the mop will be placed when it is put down.")]
    [SerializeField]
    public GameObject mopPlacement;

    [Header("Mop Taken?")]
    public static bool mopTaken = false;


    public void Interact() // This function is called when the player interacts with the mop object
    {
        if (mopTaken) // If the mop has already been taken, put it down
        {
            mopTaken = false;
            mopPlacement.SetActive(false);
            mopInventory.SetActive(false);
            mop.SetActive(true);
            interactionPromt.SetActive(false);
            return;
        }
        else // If the mop has not been taken yet, pick it up
        {
            mopTaken = true;
            mop.SetActive(false);
            interactionPromt.SetActive(false);
            mopInventory.SetActive(true);
            mopPlacement.SetActive(true);
        }
        
    }

    public string GetDescription()
    {
        if (mopTaken)
        {
            return "Put down the mop";
        }
        else
        {
            return "Pick up the mop";
        }
    }
}
