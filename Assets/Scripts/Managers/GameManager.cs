using System;
using System.Collections;
using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool lvl2bComplete;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip generalBGM;
    [SerializeField] private AudioClip level123BGM;
    [SerializeField] private AudioClip finalBossBGM;
    [SerializeField] private AudioClip deathSFX;


    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        //Persist across scene changes
        DontDestroyOnLoad(gameObject);
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void PlayGeneralBGM()
    {
        audioSource.clip = generalBGM;
        audioSource.loop = true;
        audioSource.Play();
    }
    public void Playlevel123BGM()
    {
        audioSource.clip = level123BGM;
        audioSource.loop = true;
        audioSource.Play();
    }
    public void PlayFinalBossBGM()
    {
        audioSource.clip = finalBossBGM;
        audioSource.loop = true;
        audioSource.Play();
    }
    public void PlayDedSFX()
    {
        audioSource.PlayOneShot(deathSFX);
    }
    public void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void DisableMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void Retry()
    {
        DisableMouse();
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void StopBGM()
    {
        audioSource.Stop();
    }
}
