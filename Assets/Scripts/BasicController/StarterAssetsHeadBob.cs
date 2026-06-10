using UnityEngine;

public class StarterAssetsHeadBob : MonoBehaviour
{
    [Header("References")]
    public Transform cameraRoot;
    public CharacterController controller;

    [Header("Test")]
    public bool alwaysTestBob = true;

    [Header("Bob Settings")]
    public float bobSpeed = 8f;
    public float verticalAmount = 0.15f;
    public float horizontalAmount = 0.05f;
    public float smooth = 20f;

    private Vector3 originalLocalPosition;
    private float timer;

    void Start()
    {
        if (cameraRoot == null)
        {
            Debug.LogError("HeadBob: Camera Root is not assigned.");
            enabled = false;
            return;
        }

        if (controller == null)
        {
            controller = GetComponent<CharacterController>();
        }

        originalLocalPosition = cameraRoot.localPosition;
        Debug.Log("HeadBob started on: " + gameObject.name);
    }

    void LateUpdate()
    {
        bool isMoving = alwaysTestBob;

        if (!alwaysTestBob && controller != null)
        {
            Vector3 horizontalVelocity = controller.velocity;
            horizontalVelocity.y = 0f;

            isMoving = horizontalVelocity.magnitude > 0.05f;
        }

        Vector3 targetPosition = originalLocalPosition;

        if (isMoving)
        {
            timer += Time.deltaTime * bobSpeed;

            float y = Mathf.Sin(timer) * verticalAmount;
            float x = Mathf.Cos(timer * 0.5f) * horizontalAmount;

            targetPosition = originalLocalPosition + new Vector3(x, y, 0f);
        }
        else
        {
            timer = 0f;
        }

        cameraRoot.localPosition = Vector3.Lerp(
            cameraRoot.localPosition,
            targetPosition,
            Time.deltaTime * smooth
        );
    }
}