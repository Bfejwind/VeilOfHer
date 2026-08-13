using UnityEngine;
using UnityEngine.Video;

public class CreditsVideoController : MonoBehaviour
{
    public GameObject creditsVideoPanel;
    public VideoPlayer videoPlayer;

    public void PlayCredits()
    {
        creditsVideoPanel.SetActive(true);

        videoPlayer.Stop();
        videoPlayer.Play();
    }

    public void CloseCredits()
    {
        videoPlayer.Stop();
        creditsVideoPanel.SetActive(false);
    }
}