using StarterAssets;
using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    [SerializeField] private FirstPersonController controller;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private float stepDistance = 3f;
    private float distanceSinceStep;
    private Vector3 lastPosition;

    void Update()
    {
        if (!controller.Grounded)
        {
            return;
        }
        distanceSinceStep += Vector3.Distance(transform.position,lastPosition);
        lastPosition = transform.position;
        if (distanceSinceStep >= stepDistance)
        {
            PlayFootStep();
            distanceSinceStep = 0f;
        }
    }
    private void PlayFootStep()
    {
        audioSource.PlayOneShot(footstepSounds[Random.Range(0, footstepSounds.Length)]);
    }
}
