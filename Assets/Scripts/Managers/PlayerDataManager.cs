using UnityEngine;
using TMPro;
public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance {get; private set;}
    public TextMeshProUGUI ammoDisplay;
    void Awake()
    {
        if (Instance !=null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public int savedPlayerAmmo, savedHealsLeft, savedMaxCommands;
    public float savedPlayerHealth,savedBulletDamage;
}
