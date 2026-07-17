using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

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

    public enum SpeakerPosition { Left, Right }
    public SpeakerPosition position;

    [SerializeField] 
    public RawImage leftPortraitImage;
    [SerializeField] 
    public RawImage rightPortraitImage;

    [Header("Visual Settings")]
    [SerializeField] private Color activeColor = Color.white; // Full brightness (1,1,1,1)
    [SerializeField] private Color dimmedColor = new Color(0.4f, 0.4f, 0.4f, 1f); // Dimmed brightness (0.4,0.4,0.4,1)

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
            if (position == SpeakerPosition.Left)
            {
                leftPortraitImage.color = activeColor;
                rightPortraitImage.color = dimmedColor;
            }
            else if (position == SpeakerPosition.Right)
            {
                rightPortraitImage.color = activeColor;
                leftPortraitImage.color = dimmedColor;
            }
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void NextLine()
    {
        if (index < dialogueLines.Length - 1)
        {
            index++;
            if (position == SpeakerPosition.Left)
            {
                position = SpeakerPosition.Right;
            }
            else if (position == SpeakerPosition.Right)
            {
                position = SpeakerPosition.Left;
            }
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
