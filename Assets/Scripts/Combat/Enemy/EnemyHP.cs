using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
public float enemyHealth,enemyHealthWidth,enemyHealthHeight;
    public float enemyMaxHealth = 100f;
    [SerializeField] private float damageAmt = 10.0f;
    public Slider healthSlider;

    void Start()
    {
        enemyHealth = enemyMaxHealth;
        healthSlider.maxValue = enemyMaxHealth;
        healthSlider.value = enemyHealth;
    }
    public void DealDamage(float amount)
    {
        enemyHealth -= amount;
        healthSlider.value = enemyHealth;
        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            DealDamage(damageAmt);
        }
    }
}
