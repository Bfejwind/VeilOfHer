using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Video;

public class ActivateDialogue : MonoBehaviour, IInteractable
{
    [SerializeField]
    GameObject dialogueBox;

    [SerializeField]
    GameObject interactionPromt;

    [SerializeField]
    public string interactionText;

    public TextMeshProUGUI dialogueText;
    public string[] dialogueLines;

    [Header("Dialogue Audio")]
    [SerializeField]
    private AudioSource dialogueAudioSource;

    [Header("Dialogue Video")]
    [SerializeField] private DialogueVideoController videoController;
    [SerializeField] private VideoClip[] dialogueVideoClips;
    [SerializeField] private bool[] dialogueVideoLoops;
    [SerializeField] private float[] dialogueVideoSpeeds;

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
    [SerializeField] private UnityEvent onDialogueStarted;
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

        if (interactionPromt != null)
        {
            interactionPromt.SetActive(false);
        }

        dialogueBox.SetActive(false);
        dialogueText.text = "";

        index = 0;

        // Start the video sequence.
        onDialogueStarted?.Invoke();

        StartCoroutine(StartDialogueAfterVideo());
    }

    public string GetDescription()
    {
        return interactionText;
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

        // Check whether this line needs to switch to a new video.
        bool videoIsChanging = PlayCurrentVideoClip();

        if (videoIsChanging)
        {
            // Hide dialogue while the video fades and prepares.
            dialogueBox.SetActive(false);

            while (!videoController.IsVideoReady)
            {
                yield return null;
            }

            // New video is ready.
            dialogueBox.SetActive(true);
        }
        else
        {
            // No video, or the same video is continuing.
            // Show the dialogue immediately.
            dialogueBox.SetActive(true);
        }

        UpdatePortraitVisibility();
        PlayCurrentVoiceLine();

        foreach (char character in currentLine)
        {
            dialogueText.text += character;
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

    private bool PlayCurrentVideoClip()
    {
        if (videoController == null)
        {
            return false;
        }

        if (dialogueVideoClips == null ||
            index < 0 ||
            index >= dialogueVideoClips.Length)
        {
            return false;
        }

        VideoClip currentVideo = dialogueVideoClips[index];

        if (currentVideo == null)
        {
            return false;
        }

        bool shouldLoop = true;

        if (dialogueVideoLoops != null &&
            index < dialogueVideoLoops.Length)
        {
            shouldLoop = dialogueVideoLoops[index];
        }

        float playbackSpeed = 1f;

        if (dialogueVideoSpeeds != null &&
            index < dialogueVideoSpeeds.Length &&
            dialogueVideoSpeeds[index] > 0f)
        {
            playbackSpeed = dialogueVideoSpeeds[index];
        }

        return videoController.PlayClip(
            currentVideo,
            shouldLoop,
            playbackSpeed);
    }

    private IEnumerator StartDialogueAfterVideo()
    {
        dialogueActive = true;
        index = 0;

        yield return StartCoroutine(TypeLine());
    }
}
