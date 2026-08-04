using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float playerHealth,playerHealthWidth,playerHealthHeight;
    public float playerMaxHealth = 100f;
    public Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    [Header("Healing")]
    public int currentHealNum;
    private int maxHealNum = 2;
    public float currentRecharge;
    public float maxRecharge = 100f;
    public Slider rechargeSlider;
    public TextMeshProUGUI currentHealNumText;
    [SerializeField] private float healAmount = 30.0f;
    
    [Header("DashInvulnerability")]
    //Dash Invulnerability
    public bool IsInvulnerable { get; private set; }
    // private float damageAmt = 12.0f;
    [SerializeField] private GameObject Shield;
    //ScreenShake
    [SerializeField] private ScreenShakeEffects screenShake;
    [Header("Audio")]
    [SerializeField] private PlayerAudio playerAudio;
    void Awake()
    {
        if (playerAudio == null)
        {
            playerAudio = GetComponent<PlayerAudio>();
        }
    }

    void Start()
    {
        currentHealNum = maxHealNum;
        currentRecharge = 0f;
        rechargeSlider.maxValue = maxRecharge;
        rechargeSlider.value = currentRecharge;
        currentHealNumText.text = currentHealNum.ToString();
        playerHealth = playerMaxHealth;
        healthSlider.maxValue = playerMaxHealth;
        UpdateHealthSlider();
        if (screenShake == null)
        {
            screenShake = Camera.main.GetComponent<ScreenShakeEffects>();
        }
    }
    public void TakeDamage(float amount)
    {
        playerHealth -= amount;
        UpdateHealthSlider();
        StartCoroutine(DashInvulnerability(0.5f));
        playerAudio.PlayerHurt();
    }
    public void OnHeal()
    {
        if (currentHealNum > 0 && playerHealth < playerMaxHealth)
        {
            playerHealth += healAmount;
            UpdateHealthSlider();
            playerAudio.PlayHealSFX();
            currentHealNum--;
            currentHealNumText.text = currentHealNum.ToString();
        }
    }
    public void RechargeHeal(float amount)
    {
        if (currentHealNum < maxHealNum)
        {
            currentRecharge += amount;
            rechargeSlider.value = currentRecharge;
            playerAudio.PlayHealOrbPickUpSFX();
            if (currentRecharge >= maxRecharge)
            {
                currentRecharge = 0f;
                rechargeSlider.value = currentRecharge;
                currentHealNum++;
                currentHealNumText.text = currentHealNum.ToString();
                playerAudio.PlayHealRechargedSFX();
            }
            
        }
    }
    public IEnumerator DashInvulnerability(float duration)
    {
        IsInvulnerable = true;

        yield return new WaitForSeconds(duration);

        IsInvulnerable = false;
    }
    public void CameraEffect()
    {
        screenShake.ScreenShake();
    }
    private void UpdateHealthSlider()
    {
        playerHealth = Mathf.Clamp(playerHealth, 0f, playerMaxHealth);
        healthSlider.value = playerHealth;
        healthText.text = playerHealth + "/" + playerMaxHealth;
    }
    // void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Bullet"))
    //     {
    //         if (Shield.activeSelf)
    //         {
    //             Debug.Log("Shielded");
    //         }
    //         else
    //         {
    //             Debug.Log("Hit");
    //             TakeDamage(damageAmt);
    //         }
    //     }
    // }
}
