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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController.canMove = false;
        playerShoot.readyToShoot = false;
        bossEntranceVid.loopPointReached += VideoFinished;
    }
    private void VideoFinished(VideoPlayer vp)
    {
        playerController.canMove = true;
        playerShoot.readyToShoot = true;
        vidGameObj.SetActive(false);
    }
}
