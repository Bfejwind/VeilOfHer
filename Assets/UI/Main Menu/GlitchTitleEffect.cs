using UnityEngine;
using TMPro;

public class GlitchTitleEffect : MonoBehaviour
{
    [Header("Text Layers")]
    public RectTransform mainText;
    public RectTransform cyanText;
    public RectTransform magentaText;
    public CanvasGroup titleCanvasGroup;

    [Header("Glitch Settings")]
    public float glitchInterval = 0.08f;
    public float glitchChance = 0.25f;
    public float offsetAmount = 6f;

    private Vector2 cyanOriginalPos;
    private Vector2 magentaOriginalPos;
    private float timer;

    void Start()
    {
        cyanOriginalPos = cyanText.anchoredPosition;
        magentaOriginalPos = magentaText.anchoredPosition;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= glitchInterval)
        {
            timer = 0f;

            if (Random.value < glitchChance)
            {
                DoGlitch();
            }
            else
            {
                ResetGlitch();
            }
        }
    }

    void DoGlitch()
    {
        cyanText.anchoredPosition = cyanOriginalPos + new Vector2(
            Random.Range(-offsetAmount, offsetAmount),
            Random.Range(-2f, 2f)
        );

        magentaText.anchoredPosition = magentaOriginalPos + new Vector2(
            Random.Range(-offsetAmount, offsetAmount),
            Random.Range(-2f, 2f)
        );

        float scaleJitter = Random.Range(0.98f, 1.03f);
        mainText.localScale = new Vector3(scaleJitter, 1f, 1f);

        titleCanvasGroup.alpha = Random.Range(0.75f, 1f);
    }

    void ResetGlitch()
    {
        cyanText.anchoredPosition = cyanOriginalPos;
        magentaText.anchoredPosition = magentaOriginalPos;
        mainText.localScale = Vector3.one;

        titleCanvasGroup.alpha = 1f;
    }
}