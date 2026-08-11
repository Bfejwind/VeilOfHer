using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ActivateWater : MonoBehaviour, IInteractable
{
    [Header("Water Particles")]
    [Tooltip("The water particles that plays when watering the plants.")]
    [SerializeField]
    public GameObject waterParticles;

    [Header("Interaction UI")]
    [Tooltip("The interaction element that displays status messages.")]
    [SerializeField]
    public GameObject interactionPromt;

    [Header("Water Events")]
    [SerializeField] private UnityEvent CompletedWaterObjective;

    public void Interact()
    {
        if (waterParticles != null)
        {
            waterParticles.SetActive(true);
            StartCoroutine(wait());
            CompletedWaterObjective?.Invoke(); // Invoke the CompletedWaterObjective event to notify other scripts that the water objective has been completed
        }
    }

    public string GetDescription()
    {
        return "Water the plants";
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(5f);
        if (waterParticles != null)
        {
            waterParticles.SetActive(false);
        }
    }
}
