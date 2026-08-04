using UnityEngine;
using UnityEngine.Video;

public class BreakfastVideoController : MonoBehaviour
{
    [Header("Video")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject videoOverlay;

    [Header("Controls Disabled During Video")]
    [SerializeField] private MonoBehaviour[] controlsToDisable;

    private bool breakfastVideoPlaying;

    private void Awake()
    {
        if (videoOverlay != null)
        {
            videoOverlay.SetActive(false);
        }

        if (videoPlayer != null)
        {
            videoPlayer.playOnAwake = false;
            videoPlayer.isLooping = true;
        }
    }

    public void StartBreakfastVideo()
    {
        if (breakfastVideoPlaying)
        {
            return;
        }

        breakfastVideoPlaying = true;

        SetControlsEnabled(false);

        if (videoOverlay != null)
        {
            videoOverlay.SetActive(true);
        }

        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.frame = 0;
            videoPlayer.Play();
        }
    }

    public void EndBreakfastVideo()
    {
        if (!breakfastVideoPlaying)
        {
            return;
        }

        breakfastVideoPlaying = false;

        if (videoPlayer != null)
        {
            videoPlayer.Stop();
        }

        if (videoOverlay != null)
        {
            videoOverlay.SetActive(false);
        }

        SetControlsEnabled(true);
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