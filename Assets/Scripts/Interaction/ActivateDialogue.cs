using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class ActivateDialogue : MonoBehaviour, IInteractable
{
    [SerializeField]
    GameObject dialogueBox;

    [SerializeField]
    GameObject interactionPromt;

    public TextMeshProUGUI dialogueText;
    public string[] dialogueLines;

    [Header("Dialogue Audio")]
    [SerializeField] 
    private AudioSource dialogueAudioSource;

    [SerializeField] 
    private AudioClip[] dialogueAudioClips;

    public float textSpeed;
    public bool dialogueActive = false;

    public enum SpeakerPosition { Left, Right }
    public SpeakerPosition position;

    [SerializeField]
    public RawImage leftPortraitImage;
    [SerializeField]
    public RawImage rightPortraitImage;

    [Header("Dialogue Events")]
    [SerializeField] private UnityEvent onDialogueFinished;

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

        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        bool textFinished =
            dialogueText.text == dialogueLines[index];

        if (!textFinished)
        {
            // First click: reveal the full subtitle,
            // but allow the voice to continue.
            StopAllCoroutines();
            dialogueText.text = dialogueLines[index];
            return;
        }

        // Second click: stop the current voice and go forward.
        if (dialogueAudioSource != null)
        {
            dialogueAudioSource.Stop();
        }

        NextLine();
    }

    private void PlayCurrentVoiceLine()
    {
        if (dialogueAudioSource == null)
        {
            return;
        }

        dialogueAudioSource.Stop();

        if (dialogueAudioClips == null ||
            index < 0 ||
            index >= dialogueAudioClips.Length)
        {
            return;
        }

        AudioClip currentClip = dialogueAudioClips[index];

        if (currentClip == null)
        {
            return;
        }

        dialogueAudioSource.clip = currentClip;
        dialogueAudioSource.Play();
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

        UpdatePortraitVisibility();
        PlayCurrentVoiceLine();

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

            if (leftPortraitImage != null)
            {
                leftPortraitImage.gameObject.SetActive(false);
            }

            if (rightPortraitImage != null)
            {
                rightPortraitImage.gameObject.SetActive(false);
            }

            onDialogueFinished?.Invoke();
            
            if (dialogueAudioSource != null)
            {
                dialogueAudioSource.Stop();
            }
        }
    }

    private void UpdatePortraitVisibility()
    {
        if (leftPortraitImage == null || rightPortraitImage == null)
        {
            Debug.LogWarning("One or both dialogue portraits are not assigned.");
            return;
        }

        bool zaneIsSpeaking = position == SpeakerPosition.Left;

        leftPortraitImage.gameObject.SetActive(zaneIsSpeaking);
        rightPortraitImage.gameObject.SetActive(!zaneIsSpeaking);
    }
}
