using UnityEngine;

public class ActivateDoor : MonoBehaviour, IInteractable
{
    Transition transition;

    [Header("Target Scene")]
    [SerializeField]
    public string targetSceneName; // The name of the scene to load when the door is activated

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transition = GetComponent<Transition>();
    }

    public void Interact()
    {
        transition.StartTransition(targetSceneName);
    }

    public string GetDescription()
    {
        return "Enter deeper into Zyr4's head";
    }
}
