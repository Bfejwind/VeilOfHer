using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class DialogueVideoController : MonoBehaviour
{
    [Header("Video")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject videoOverlay;
    [SerializeField] private RenderTexture videoRenderTexture;

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
    }

    public void PlayClip(
        VideoClip newClip,
        bool shouldLoop,
        float playbackSpeed)
    {
        if (newClip == null || videoPlayer == null)
        {
            return;
        }

        if (currentClip == newClip)
        {
            return;
        }

        if (switchCoroutine != null)
        {
            StopCoroutine(switchCoroutine);
        }

        switchCoroutine = StartCoroutine(
            SwitchClipRoutine(newClip, shouldLoop, playbackSpeed));
    }

    private IEnumerator SwitchClipRoutine(
        VideoClip newClip,
        bool shouldLoop,
        float playbackSpeed)
    {
        // Cover the previous visual first.
        yield return Fade(0f, 1f);

        if (videoPlayer == null)
        {
            yield break;
        }

        videoPlayer.Stop();

        // Remove the previous repair/breakfast frame.
        ClearVideoRenderTexture();

        videoPlayer.clip = newClip;
        videoPlayer.isLooping = shouldLoop;
        videoPlayer.playbackSpeed = Mathf.Max(0.1f, playbackSpeed);

        currentClip = newClip;

        // The overlay is only shown while the black fade covers it.
        if (videoOverlay != null)
        {
            videoOverlay.SetActive(true);
        }

        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        videoPlayer.Play();

        // Wait until the VideoPlayer has produced its first real frame.
        while (videoPlayer.frame < 0)
        {
            yield return null;
        }

        // Give the Render Texture one frame to update.
        yield return null;

        // Reveal the correct new clip.
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
        ClearVideoRenderTexture();

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

    private void ClearVideoRenderTexture()
    {
        if (videoRenderTexture == null)
        {
            return;
        }

        RenderTexture previousActiveTexture = RenderTexture.active;

        RenderTexture.active = videoRenderTexture;
        GL.Clear(true, true, Color.black);

        RenderTexture.active = previousActiveTexture;
    }
}