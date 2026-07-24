using UnityEngine;

public class FinalBossAudio : MonoBehaviour
{
    [SerializeField] public AudioSource audioSource;
    [SerializeField] private AudioClip teleportOut;
    [SerializeField] private AudioClip teleportIn;

    public void PlayTeleportOut()
    {
        audioSource.PlayOneShot(teleportOut);
    }
    public void PlayTeleportIn()
    {
        audioSource.PlayOneShot(teleportIn);
    }
}
