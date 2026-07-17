using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
public float enemyHealth,enemyHealthWidth,enemyHealthHeight;
    public float enemyMaxHealth = 100f;
    public float damageAmt;
    public Slider healthSlider;
    public bool shotAt;

    void Start()
    {
        shotAt = false;
        enemyHealth = enemyMaxHealth;
        healthSlider.maxValue = enemyMaxHealth;
        healthSlider.value = enemyHealth;
    }
    public void TakingDamage(float amount)
    {
        shotAt = true;
        enemyHealth -= amount;
        healthSlider.value = enemyHealth;
        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
