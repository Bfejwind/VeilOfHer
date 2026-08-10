using UnityEngine;
using TMPro;
public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance {get; private set;}
    public TextMeshProUGUI ammoDisplay;
    [Header("Stats")]
    public int savedHealsLeft, savedMaxCommands;
    public float savedPlayerHealth,savedBulletDamage;
    [Header("UI")]
    public int savedPlayerAmmo;
    public float savedHealRechargeSlider;
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
}
