using System;
using System.Collections;
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
}
