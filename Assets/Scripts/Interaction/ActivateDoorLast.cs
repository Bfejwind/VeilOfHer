using StarterAssets;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ActivateDoorLast : MonoBehaviour, IInteractable
{
    Transition transition;

    [Header("Target Scene")]
    [SerializeField]
    public string targetSceneName; // The name of the scene to load when the door is activated

    [SerializeField]
    public GameObject finalDecision;
    [SerializeField] private InputActionReference cmd;
    [SerializeField] private FirstPersonController playerController;
    [SerializeField] private Weapon playerShoot;

    [Header("Interaction Description")]
    [Tooltip("Description of the interaction for UI purposes.")]
    [SerializeField]
    public string description;

    [Header("Door Events")]
    [SerializeField] 
    private UnityEvent completedTask;
    public void Interact()
    {
        GameManager.Instance.StopBGM();
        playerController.canMove = false;
        playerShoot.readyToShoot = false;
        cmd.action.Disable();
        finalDecision.SetActive(true);
        completedTask?.Invoke();
        Time.timeScale = 0;
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
