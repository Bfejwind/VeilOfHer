using UnityEngine;
using TMPro;
using System.Collections;

public class ActivateDialogue : MonoBehaviour, IInteractable
{   
    [SerializeField]
    GameObject dialogueBox;

    [SerializeField]
    GameObject interactionPromt;

    public TextMeshProUGUI dialogueText;
    public string[] dialogueLines;
    public float textSpeed;
    public bool dialogueActive = false;

    private int index;

    public void Interact()
    {
        if (dialogueActive)
        {
            return;
        }
        else
        {
            dialogueText.text = "";
            StartDialogue();
            dialogueBox.SetActive(true);
            interactionPromt.SetActive(false);
        }
        
    }

    public string GetDescription()
    {
        return "Talk to Zyr4";
    }

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (dialogueText.text == dialogueLines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[index];
            }
        }
    }

    public void StartDialogue()
    {
        dialogueActive = true;
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in dialogueLines[index].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void NextLine()
    {
        if (index < dialogueLines.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogueBox.SetActive(false);
            dialogueActive = false;
        }
    }
}
