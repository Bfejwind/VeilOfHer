using UnityEngine;
using System.Collections;
using StarterAssets;
using UnityEngine.Events;

public class Mementos : MonoBehaviour, IInteractable
{
    [Header("Unique ID")]
    [Tooltip("Must be unique across ALL mementos in the game")]
    [SerializeField] 
    private string mementoID;

    [Header("Sparkle Object")]
    [SerializeField]
    public GameObject Sparkle;

    [Header("Memento Object")]
    [SerializeField]
    public GameObject MementoFound;

    [SerializeField]
    public GameObject MementoNotFound;
    [Header("Interaction UI")]
    [SerializeField]
    public GameObject interactionPromt;

    [Header("Memento found?")]
    public bool mementoFound = false;

    [Header("Memento Canvas")]
    public GameObject mementoCanvas;
    public GameObject currentMemento;

    StarterAssetsInputs input;

    [Header("Memento Events")]
    [SerializeField] private UnityEvent CollectedMemento;

    void Start()
    {
        input = GetComponent<StarterAssetsInputs>();

        // Check persistent state as soon as this object loads
        if (MementosManager.Instance != null && MementosManager.Instance.HasCollected(mementoID))
        {
            mementoFound = true;
            gameObject.SetActive(false); // hide the whole object, sparkle, prompt, everything
            return; // skip the rest of Start(), no point setting anything else up
        }
    }

    public void MementoInventory() // This function is called when the player interacts with the memento object
    {
        MementoFound.SetActive(true);
        MementoNotFound.SetActive(false);
        Sparkle.SetActive(false);
    }

    public void OpenMementoUI()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (input != null)
        {
            input.enabled = false;
        }
    }

    public void Interact() // This function is called when the player interacts with the memento object
    {
        if (!mementoFound) // If the memento has not been found yet
        {
            MementoInventory(); // Call the MementoInventory function to update the UI
            mementoFound = true; // Set the mementoFound variable to true so that the player cannot pick up the memento again
            currentMemento.SetActive(true);
            mementoCanvas.SetActive(true);
            OpenMementoUI();

            MementosManager.Instance.Collect(mementoID); // persist it
            CollectedMemento?.Invoke(); // Invoke the CollectedMemento event to notify other scripts that the memento has been collected
        }
    }

    public string GetDescription() // This function is called when the player looks at the memento object
    {
        return "Pick up"; // Return the description of the memento object
    }
}
