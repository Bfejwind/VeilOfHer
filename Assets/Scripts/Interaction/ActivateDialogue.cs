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

        if (dialogueBox == null)
        {
            Debug.LogError("Dialogue Box is not assigned.");
            return;
        }

        if (dialogueText == null)
        {
            Debug.LogError("Dialogue Text is not assigned.");
            return;
        }

        if (dialogueLines == null || dialogueLines.Length == 0)
        {
            Debug.LogError("Dialogue Lines are empty.");
            return;
        }

        // Open the UI first.
        dialogueBox.SetActive(true);

        if (interactionPromt != null)
        {
            interactionPromt.SetActive(false);
        }

        dialogueText.text = "";
        StartDialogue();
    }

    public string GetDescription()
    {
        return "Talk to Zyr4";
    }

    // Update is called once per frame
    private void Update()
    {
        if (!dialogueActive)
        {
            return;
        }

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

    private IEnumerator TypeLine()
    {
        if (index < 0 || index >= dialogueLines.Length)
        {
            Debug.LogError($"Invalid dialogue index: {index}");
            yield break;
        }

        string currentLine = dialogueLines[index];

        Debug.Log($"Typing dialogue line {index}: {currentLine}");

        dialogueText.text = "";

        UpdatePortraitBrightness();

        foreach (char character in currentLine)
        {
            dialogueText.text += character;

            // Continues even if dialogue pauses the game.
            yield return new WaitForSecondsRealtime(textSpeed);
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

    private void UpdatePortraitBrightness()
    {
        if (leftPortraitImage == null || rightPortraitImage == null)
        {
            return;
        }

        if (position == SpeakerPosition.Left)
        {
            leftPortraitImage.color = activeColor;
            rightPortraitImage.color = dimmedColor;
        }
        else
        {
            rightPortraitImage.color = activeColor;
            leftPortraitImage.color = dimmedColor;
        }
    }
}
