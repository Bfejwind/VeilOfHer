using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class DialogueVideoController : MonoBehaviour
{
    [Header("Video")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject videoOverlay;

    [Header("Fade")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 0.35f;

    [Header("Controls Disabled During Video")]
    [SerializeField] private MonoBehaviour[] controlsToDisable;

    private VideoClip currentClip;
    private bool videoSequenceActive;
    private Coroutine switchCoroutine;

    private void Awake()
    {
        if (videoOverlay != null)
        {
            videoOverlay.SetActive(false);
        }

        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.blocksRaycasts = false;
        }

        if (videoPlayer != null)
        {
            videoPlayer.playOnAwake = false;
            videoPlayer.isLooping = true;
        }
    }

    public void StartVideoSequence()
    {
        videoSequenceActive = true;
        SetControlsEnabled(false);

        if (videoOverlay != null)
        {
            videoOverlay.SetActive(true);
        }
    }

    public void PlayClip(VideoClip newClip, bool shouldLoop)
    {
        if (newClip == null || videoPlayer == null)
        {
            return;
        }

        // If this exact clip is already playing, keep it running.
        // This allows dialogue lines 1, 2 and 3 to share Clip 2.
        if (currentClip == newClip && videoPlayer.isPlaying)
        {
            return;
        }

        if (switchCoroutine != null)
        {
            StopCoroutine(switchCoroutine);
        }

        switchCoroutine =
            StartCoroutine(SwitchClipRoutine(newClip, shouldLoop));
    }

    private IEnumerator SwitchClipRoutine(
        VideoClip newClip,
        bool shouldLoop)
    {
        yield return Fade(0f, 1f);

        videoPlayer.Stop();
        videoPlayer.clip = newClip;
        videoPlayer.isLooping = shouldLoop;

        currentClip = newClip;

        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        videoPlayer.Play();

        yield return Fade(1f, 0f);

        switchCoroutine = null;
    }

    public void EndVideoSequence()
    {
        if (!videoSequenceActive)
        {
            return;
        }

        StartCoroutine(EndVideoRoutine());
    }

    private IEnumerator EndVideoRoutine()
    {
        yield return Fade(0f, 1f);

        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.clip = null;
        }

        currentClip = null;

        if (videoOverlay != null)
        {
            videoOverlay.SetActive(false);
        }

        videoSequenceActive = false;
        SetControlsEnabled(true);

        yield return Fade(1f, 0f);
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        if (fadeCanvasGroup == null)
        {
            yield break;
        }

        fadeCanvasGroup.blocksRaycasts = true;
        fadeCanvasGroup.alpha = startAlpha;

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float progress =
                Mathf.Clamp01(elapsed / fadeDuration);

            fadeCanvasGroup.alpha =
                Mathf.Lerp(startAlpha, endAlpha, progress);

            yield return null;
        }

        fadeCanvasGroup.alpha = endAlpha;
        fadeCanvasGroup.blocksRaycasts = endAlpha > 0f;
    }

    private void SetControlsEnabled(bool enabled)
    {
        foreach (MonoBehaviour control in controlsToDisable)
        {
            if (control != null)
            {
                control.enabled = enabled;
            }
        }
    }
}