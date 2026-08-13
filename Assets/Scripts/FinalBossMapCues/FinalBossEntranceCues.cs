using System.Collections;
using StarterAssets;
using UnityEngine;
using UnityEngine.Video;

public class FinalBossEntranceCues : MonoBehaviour
{
    [SerializeField] private FirstPersonController playerController;
    [SerializeField] private Weapon playerShoot;
    [SerializeField] private VideoPlayer bossEntranceVid;
    [SerializeField] private GameObject vidGameObj;
    [SerializeField] private GameObject bossObj;
    [SerializeField] private GameObject[] tutorialScreens;
    private int currentTutorial = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController.canMove = false;
        playerShoot.readyToShoot = false;
        bossObj.SetActive(false);
        bossEntranceVid.loopPointReached += VideoFinished;
        currentTutorial = 0;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && bossEntranceVid.isPlaying)
        {
            SkipVideo();
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            NextScreen();
        }
    }
    private void VideoFinished(VideoPlayer vp)
    {
        Debug.Log("FinishedVid");
        playerController.canMove = true;
        playerShoot.readyToShoot = true;
        bossObj.SetActive(true);
        GameManager.Instance.PlayFinalBossBGM();
        vidGameObj.SetActive(false);
    }
    public void PlayVideo()
    {
        bossEntranceVid.Play();
    }
    public void SkipVideo()
    {
        if (bossEntranceVid.isPlaying)
        {
            bossEntranceVid.Stop();
            VideoFinished(bossEntranceVid);
        }
    }
    private void NextScreen()
    {
        if (tutorialScreens.Length > 0)
        {
            tutorialScreens[currentTutorial].SetActive(false);

            currentTutorial++;

            if (currentTutorial < tutorialScreens.Length)
            {
                tutorialScreens[currentTutorial].SetActive(true);
            }
            else
            {
                PlayVideo();
            }
        }
    }
}
