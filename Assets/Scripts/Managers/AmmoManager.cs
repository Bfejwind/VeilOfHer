using TMPro;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    public static AmmoManager Instance { get; private set; }
    //UI
    public TextMeshProUGUI ammoDisplay;
    void Awake()
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
