using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool lvl2bComplete;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip generalBGM;
    [SerializeField] private AudioClip meleeBossBGM;
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
    
}
