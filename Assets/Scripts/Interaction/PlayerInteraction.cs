using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float interactionRange = 3f;

    public GameObject interactionPrompt;
    public TextMeshProUGUI interactionText;


    public void Start()
    {
        interactionPrompt.SetActive(false);
    }

    private void Update()
    {
        InteractionRay();
    }

    void InteractionRay() // This function casts a ray from the center of the player's camera to detect interactable objects
    {
        Ray ray = playerCamera.ViewportPointToRay(Vector3.one * 0.5f); // Cast a ray from the center of the camera's viewport
        RaycastHit hit; // Store information about what the ray hits

        bool hitSomething = false; // Flag to track if the ray hit an interactable object

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>(); 
            
            ActivateDialogue dialogueScript = hit.collider.GetComponent<ActivateDialogue>();

            if (interactable != null)
            {
                if (dialogueScript != null && dialogueScript.dialogueActive)
                {
                    hitSomething = false;
                }
                else
                {
                    hitSomething = true;
                    interactionText.text = interactable.GetDescription();

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        interactable.Interact();
                    }
                }
            }
        }

        interactionPrompt.SetActive(hitSomething);
    }
}
