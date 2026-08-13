using UnityEngine;
using UnityEngine.Events;

public class ActivateDoorLast : MonoBehaviour, IInteractable
{
    Transition transition;

    [Header("Target Scene")]
    [SerializeField]
    public string targetSceneName; // The name of the scene to load when the door is activated

    [SerializeField]
    public GameObject finalDecision;

    [Header("Interaction Description")]
    [Tooltip("Description of the interaction for UI purposes.")]
    [SerializeField]
    public string description;

    [Header("Door Events")]
    [SerializeField] 
    private UnityEvent completedTask;

    public void Interact()
    {
        finalDecision.SetActive(true);
    }

    public string GetDescription()
    {
        return description;
    }

    public void TransitionToMainMenu()
    {  
        Transition.Instance.StartTransition(targetSceneName);
    }
}
