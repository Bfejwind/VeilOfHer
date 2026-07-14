using TMPro;
using UnityEngine;

public class WaypointUI : MonoBehaviour
{
    [Header("World Objects")]
    [SerializeField] private Transform target;
    [SerializeField] private Transform player;
    [SerializeField] private Camera mainCamera;

    [Header("UI")]
    [SerializeField] private RectTransform waypointRect;
    [SerializeField] private TMP_Text distanceText;
    [SerializeField] private CanvasGroup waypointCanvasGroup;

    [Header("Settings")]
    [SerializeField] private Vector3 targetOffset = Vector3.zero;
    [SerializeField] private float arrivalDistance = 2f;
    [SerializeField] private bool waypointReached;

    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void LateUpdate()
    {
        if (target == null ||
            player == null ||
            mainCamera == null ||
            waypointRect == null ||
            waypointCanvasGroup == null)
        {
            return;
        }

        CheckIfWaypointReached();

        if (waypointReached)
        {
            waypointCanvasGroup.alpha = 0f;
            return;
        }

        UpdateWaypointPosition();
        UpdateDistanceText();
    }

    private void CheckIfWaypointReached()
    {
        float distance = Vector3.Distance(
            player.position,
            target.position
        );

        if (distance <= arrivalDistance)
        {
            waypointReached = true;
        }
    }

    private void UpdateWaypointPosition()
    {
        Vector3 targetWorldPosition =
            target.position + targetOffset;

        Vector3 screenPosition =
            mainCamera.WorldToScreenPoint(targetWorldPosition);

        bool targetIsBehindCamera =
            screenPosition.z < 0f;

        waypointCanvasGroup.alpha =
            targetIsBehindCamera ? 0f : 1f;

        if (targetIsBehindCamera)
        {
            return;
        }

        waypointRect.position = screenPosition;
    }

    private void UpdateDistanceText()
    {
        float distance = Vector3.Distance(
            player.position,
            target.position
        );

        distanceText.text = $"{distance:0} m";
    }
}