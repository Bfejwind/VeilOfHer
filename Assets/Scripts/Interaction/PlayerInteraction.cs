using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float interactionRange = 3f;

    public GameObject interactionPrompt;
    public TextMeshProUGUI interactionText;

    private void Update()
    {
        InteractionRay();
    }

    void InteractionRay()
    {
        Ray ray = playerCamera.ViewportPointToRay(Vector3.one * 0.5f);
        RaycastHit hit;

        bool hitSomething = false;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                hitSomething = true;
                interactionText.text = interactable.GetDescription();

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();
                }
            }
        }

        interactionPrompt.SetActive(hitSomething);
    }
}
