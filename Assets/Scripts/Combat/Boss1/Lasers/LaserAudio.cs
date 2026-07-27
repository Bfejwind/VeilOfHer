using UnityEngine;

public class LaserAudio : MonoBehaviour
{
    [SerializeField] private AudioSource laserLoopSource;
    [SerializeField] private AudioClip laserConstantSFX;

    private void OnEnable()
    {
        ConstantLaserSound();
    }
    private void ConstantLaserSound()
    {
        laserLoopSource.clip = laserConstantSFX;
        laserLoopSource.loop = true;
        laserLoopSource.Play();
    }
}
