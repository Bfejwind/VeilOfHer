using UnityEngine;

public class Perimeter : MonoBehaviour
{

    [Header("Cleaning Objects")]
    [Tooltip("The mop object that the player can pick up.")]
    [SerializeField]
    public GameObject mop;

    [Header("Inventory Objects")]
    [Tooltip("The inventory object that represents the mop in the player's inventory.")]
    [SerializeField]
    public GameObject mopInventory;

    [Header("Mop Placement")]
    [Tooltip("The location where the mop will be placed when it is put down.")]
    [SerializeField]
    public GameObject mopPlacement;


    public void Start()
    {
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (ActivateCleaning.mopTaken) // If the mop has already been taken, put it down
        {
            ActivateCleaning.mopTaken = false;
            mopPlacement.SetActive(false);
            mopInventory.SetActive(false);
            mop.SetActive(true);
            return;
        }
    }
}
