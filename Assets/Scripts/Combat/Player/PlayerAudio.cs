using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("Source")]
    [SerializeField] private AudioSource audioSource;
    [Header("Hurt")]
    [SerializeField] private AudioClip[] hurtSFX;
    [Header("Healing")]
    [SerializeField] private AudioClip healOrbPickUpSFX;
    [SerializeField] private AudioClip healRechargedSFX;
    [SerializeField] private AudioClip healSFX;
    [Header("Dash")]
    [SerializeField] private AudioSource mutedSource;
    [SerializeField] private AudioClip dashSFX;
    [SerializeField] private AudioClip dashThroughSFX;

    public void PlayerHurt()
    {
        audioSource.PlayOneShot(hurtSFX[Random.Range(0, hurtSFX.Length)]);
    }

    public void PlayHealOrbPickUpSFX()
    {
        audioSource.PlayOneShot(healOrbPickUpSFX);
    }

    public void PlayHealRechargedSFX()
    {
        audioSource.PlayOneShot(healRechargedSFX);
    }
    public void PlayHealSFX()
    {
        audioSource.PlayOneShot(healSFX);
    }
    public void PlayDashSFX()
    {
        mutedSource.PlayOneShot(dashSFX);
    }
    public void PlayDashThroughSFX()
    {
        audioSource.PlayOneShot(dashThroughSFX);
    }

}
