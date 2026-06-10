using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    Dialogue dialogue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogue = GetComponent<Dialogue>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            dialogue.StartDialogue();
        }
    }

    void PickUpItem()
    {
        // Implement item pickup logic here
    }
}
