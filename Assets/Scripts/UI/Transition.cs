using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System;

public class Transition : MonoBehaviour
{
     [Header("Canvas Groups")]
    [SerializeField] private CanvasGroup whiteScreenGroup;
    [SerializeField] private CanvasGroup logoGroup;

    [SerializeField] public GameObject transitionCanvas;
    
    [Header("Video References")]
    [SerializeField] private GameObject logoObject;
    [SerializeField] private VideoPlayer videoPlayer;

    [Header("Timing Settings")]
    [SerializeField] private float whiteFadeDuration = 1f;
    [SerializeField] private float logoFadeDuration = 0.5f;
    [SerializeField] private float minimumVideoPlayTime = 5.0f;

    [Header("Target Scene")]
    [SerializeField]
    public Scene targetScene;

    public static Transition Instance;

    private bool isTransitioning = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        transform.SetParent(null); // ensure it's a root object before persisting
        DontDestroyOnLoad(gameObject);
        
        // Initial States
        whiteScreenGroup.alpha = 0f;
        logoGroup.alpha = 0f;
        logoObject.SetActive(false);
    }

    public void StartTransition(string targetScene)
    {
        if (isTransitioning) return;
        isTransitioning = true;

        transitionCanvas.SetActive(true);
        Time.timeScale = 0f; // Pause the game during the transition
        StartCoroutine(TransitionSequence(targetScene));
        Debug.Log("Transition started to scene: " + targetScene);
    }

    private IEnumerator TransitionSequence(string targetScene)
    {
        // 1. Fade the screen to white
        yield return StartCoroutine(FadeCanvas(whiteScreenGroup, 0f, 1f, whiteFadeDuration));

        // 2. Prepare and play the video loop
        logoObject.SetActive(true);
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }
        videoPlayer.Play();

        // 3. Fade the logo wrapper in gently over the white background
        yield return StartCoroutine(FadeCanvas(logoGroup, 0f, 1f, logoFadeDuration));

        // Start loading the scene in the background
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene);
        asyncLoad.allowSceneActivation = false;

        // 4. Keep looping video until minimum display time AND background scene load finishes
        float timer = 0f;
        while (timer < minimumVideoPlayTime || asyncLoad.progress < 0.9f)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        // 5. Fade out the logo wrapper gently (white screen stays fully visible)
        yield return StartCoroutine(FadeCanvas(logoGroup, 1f, 0f, logoFadeDuration));
        videoPlayer.Stop();
        logoObject.SetActive(false);

        // 6. Activate the new scene
        asyncLoad.allowSceneActivation = true;
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // 7. Fade white screen back to clear revealing the new scene
        yield return StartCoroutine(FadeCanvas(whiteScreenGroup, 1f, 0f, whiteFadeDuration));

        // gameObject.SetActive(false);
        isTransitioning = false;
        Time.timeScale = 1f; // Resume the game
    }

    private IEnumerator FadeCanvas(CanvasGroup group, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            group.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            yield return null;
        }
        group.alpha = endAlpha;
    }
}