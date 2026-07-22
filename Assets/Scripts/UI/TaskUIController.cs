using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskUIController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text taskText;
    [SerializeField] private Image taskIcon;

    [Header("Icons")]
    [SerializeField] private Sprite normalTaskIcon;
    [SerializeField] private Sprite completedTaskIcon;

    [Header("Completion Settings")]
    [SerializeField] private string completedText = "Task completed";
    [SerializeField] private float completedDisplayDuration = 1.2f;
    [SerializeField] private float fadeDuration = 0.35f;

    private Coroutine completionCoroutine;

    private void Awake()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }
    }

    public void ShowTask(string description)
    {
        if (completionCoroutine != null)
        {
            StopCoroutine(completionCoroutine);
            completionCoroutine = null;
        }

        gameObject.SetActive(true);

        if (taskText != null)
        {
            taskText.text = description;
        }

        if (taskIcon != null && normalTaskIcon != null)
        {
            taskIcon.sprite = normalTaskIcon;
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
        }
    }

    public void CompleteTask()
    {
        if (completionCoroutine != null)
        {
            StopCoroutine(completionCoroutine);
        }

        completionCoroutine = StartCoroutine(CompleteTaskRoutine());
    }

    private IEnumerator CompleteTaskRoutine()
    {
        if (taskIcon != null && completedTaskIcon != null)
        {
            taskIcon.sprite = completedTaskIcon;
        }

        if (taskText != null)
        {
            taskText.text = completedText;
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
        }

        yield return new WaitForSecondsRealtime(completedDisplayDuration);

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            if (canvasGroup != null)
            {
                canvasGroup.alpha =
                    Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            }

            yield return null;
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }

        gameObject.SetActive(false);
        completionCoroutine = null;
    }
}