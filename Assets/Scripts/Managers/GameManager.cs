using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Light visionRadius;

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
        // visionRadius = GetComponent<Light>();
    }
    
    public void ChangeLightRadius(float value)
    {
        visionRadius.range += value;
    }
}
