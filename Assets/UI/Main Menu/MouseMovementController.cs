using UnityEngine;

public class MenuParallaxImage : MonoBehaviour
{
    public RectTransform background;
    public float moveAmount = 30f;
    public float smoothSpeed = 5f;

    private Vector2 startPos;

    void Start()
    {
        startPos = background.anchoredPosition;
    }

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;

        float x = (mousePos.x / Screen.width - 0.5f) * 2f;
        float y = (mousePos.y / Screen.height - 0.5f) * 2f;

        Vector2 targetPos = startPos + new Vector2(-x * moveAmount, -y * moveAmount);

        background.anchoredPosition = Vector2.Lerp(
            background.anchoredPosition,
            targetPos,
            Time.deltaTime * smoothSpeed
        );
    }
}
