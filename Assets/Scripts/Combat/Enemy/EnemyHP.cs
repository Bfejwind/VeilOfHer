using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Boss1Behaviour boss1Behaviour;
    public float enemyHealth,enemyHealthWidth,enemyHealthHeight;
    public float enemyMaxHealth = 100f;
    public float damageAmt;
    public Slider healthSlider;
    public bool shotAt;
    private void Awake()
    {
        if (boss1Behaviour == null)
        {
            return;
        }
    }

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
        if (boss1Behaviour != null)
        {
            boss1Behaviour.damageTaken += amount;
        }
        healthSlider.value = enemyHealth;
        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
