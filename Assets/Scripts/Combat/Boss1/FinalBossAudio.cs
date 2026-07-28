using UnityEngine;

public class FinalBossAudio : MonoBehaviour
{
    [SerializeField] public AudioSource audioSource;
    [SerializeField] private AudioClip teleportOut;
    [SerializeField] private AudioClip teleportIn;
    [SerializeField] public AudioSource waveStartSource;
    [SerializeField] private AudioClip waveStartSFX;

    public void PlayTeleportOut()
    {
        audioSource.PlayOneShot(teleportOut);
    }
    public void PlayTeleportIn()
    {
        audioSource.PlayOneShot(teleportIn);
    }
    public void PlayWaveStartSFX()
    {
        waveStartSource.PlayOneShot(waveStartSFX);
    }
}
