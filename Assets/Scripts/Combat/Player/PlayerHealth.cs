using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float playerHealth,playerHealthWidth,playerHealthHeight;
    public float playerMaxHealth = 100f;
    public Slider healthSlider;
    private float damageAmt = 12.0f;
    [SerializeField] private GameObject Shield;

    void Start()
    {
        playerHealth = playerMaxHealth;
        healthSlider.maxValue = playerMaxHealth;
        healthSlider.value = playerHealth;
    }
    public void TakeDamage(float amount)
    {
        playerHealth -= amount;
        healthSlider.value = playerHealth;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            if (Shield.activeSelf)
            {
                Debug.Log("Shielded");
            }
            else
            {
                Debug.Log("Hit");
                TakeDamage(damageAmt);
            }
        }
    }
}
