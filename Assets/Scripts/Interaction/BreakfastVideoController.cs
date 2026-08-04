using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class BreakfastVideoController : MonoBehaviour
{
    [Header("Video")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject videoOverlay;

    [Header("Fade")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;

    [Header("Controls Disabled During Video")]
    [SerializeField] private MonoBehaviour[] controlsToDisable;

    private bool breakfastVideoPlaying;
    private bool transitionRunning;

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

    public void StartBreakfastVideo()
    {
        if (breakfastVideoPlaying || transitionRunning)
        {
            return;
        }

        StartCoroutine(StartVideoRoutine());
    }

    public void EndBreakfastVideo()
    {
        if (!breakfastVideoPlaying || transitionRunning)
        {
            return;
        }

        StartCoroutine(EndVideoRoutine());
    }

    private IEnumerator StartVideoRoutine()
    {
        transitionRunning = true;

        SetControlsEnabled(false);

        // Fade the gameplay view to black.
        yield return Fade(0f, 1f);

        if (videoOverlay != null)
        {
            videoOverlay.SetActive(true);
        }

        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.frame = 0;
            videoPlayer.isLooping = true;
            videoPlayer.Prepare();

            while (!videoPlayer.isPrepared)
            {
                yield return null;
            }

            videoPlayer.Play();
        }

        breakfastVideoPlaying = true;

        // Reveal the video.
        yield return Fade(1f, 0f);

        transitionRunning = false;
    }

    private IEnumerator EndVideoRoutine()
    {
        transitionRunning = true;

        // Cover the video.
        yield return Fade(0f, 1f);

        if (videoPlayer != null)
        {
            videoPlayer.Stop();
        }

        if (videoOverlay != null)
        {
            videoOverlay.SetActive(false);
        }

        breakfastVideoPlaying = false;

        SetControlsEnabled(true);

        // Reveal gameplay again.
        yield return Fade(1f, 0f);

        transitionRunning = false;
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        if (fadeCanvasGroup == null)
        {
            yield break;
        }

        fadeCanvasGroup.blocksRaycasts = true;

        float elapsed = 0f;
        fadeCanvasGroup.alpha = startAlpha;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float progress = Mathf.Clamp01(elapsed / fadeDuration);

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