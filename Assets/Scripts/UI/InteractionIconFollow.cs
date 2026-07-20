using UnityEngine;

public class InteractionPromptFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private RectTransform promptRect;
    [SerializeField] private Vector3 worldOffset = Vector3.zero;

    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void LateUpdate()
    {
        if (target == null || mainCamera == null || promptRect == null)
        {
            return;
        }

        Vector3 worldPosition = target.position + worldOffset;
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);

        bool isBehindCamera = screenPosition.z < 0f;

        promptRect.gameObject.SetActive(!isBehindCamera);

        if (isBehindCamera)
        {
            return;
        }

        promptRect.position = screenPosition;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}