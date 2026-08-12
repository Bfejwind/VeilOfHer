using UnityEngine;
using UnityEngine.Events;

public class ActivateDoor : MonoBehaviour, IInteractable
{
    Transition transition;

    [Header("Target Scene")]
    [SerializeField]
    public string targetSceneName; // The name of the scene to load when the door is activated

    [Header("Interaction Description")]
    [Tooltip("Description of the interaction for UI purposes.")]
    [SerializeField]
    public string description;

    [Header("Door Events")]
    [SerializeField] private UnityEvent completedTask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Interact()
    {
        Transition.Instance.StartTransition(targetSceneName);
        completedTask?.Invoke();
    }

    public string GetDescription()
    {
        return description;
    }
}
