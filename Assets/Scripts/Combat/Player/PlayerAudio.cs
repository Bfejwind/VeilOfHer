using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("Source")]
    [SerializeField] private AudioSource audioSource;
    [Header("Hurt SFX")]
    [SerializeField] private AudioClip[] hurtSFX;

    public void PlayerHurt()
    {
        audioSource.PlayOneShot(hurtSFX[Random.Range(0, hurtSFX.Length)]);
    }

}
