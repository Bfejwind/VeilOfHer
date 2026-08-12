using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonUISound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource audioSource;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip hoverClip;
    [SerializeField] private AudioClip clickClip;

    [Header("Volume")]
    [SerializeField, Range(0f, 1f)] private float hoverVolume = 0.35f;
    [SerializeField, Range(0f, 1f)] private float clickVolume = 0.6f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (audioSource != null && hoverClip != null)
        {
            audioSource.PlayOneShot(hoverClip, hoverVolume);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (audioSource != null && clickClip != null)
        {
            audioSource.PlayOneShot(clickClip, clickVolume);
        }
    }
}