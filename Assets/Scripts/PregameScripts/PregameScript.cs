using UnityEngine;
using UnityEngine.Video;

public class PregameScript : MonoBehaviour
{
    [SerializeField] private VideoPlayer pregameVid;
    private void Start()
    {
        pregameVid.loopPointReached += VideoFinished;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SkipVideo();
        }
    }
    private void VideoFinished(VideoPlayer vp)
    {
        GameManager.Instance.LoadScene("Main Menu");
    }
    public void SkipVideo()
    {
        if (pregameVid.isPlaying)
        {
            pregameVid.Stop();
            VideoFinished(pregameVid);
        }
    }
}
