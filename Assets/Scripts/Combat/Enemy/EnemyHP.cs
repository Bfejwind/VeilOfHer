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
    private HealOrbsLoot healOrbsLoot;
    public bool isInvulnerable;
    private void Awake()
    {
        if (boss1Behaviour == null)
        {
            return;
        }
    }

    void Start()
    {
        healOrbsLoot = GetComponent<HealOrbsLoot>();
        shotAt = false;
        enemyHealth = enemyMaxHealth;
        healthSlider.maxValue = enemyMaxHealth;
        healthSlider.value = enemyHealth;
    }
    public void TakingDamage(float amount)
    {
        if (!isInvulnerable)
        {
            shotAt = true;
            enemyHealth -= amount;
            Vector3 randomPopUp = new Vector3(Random.Range(0f,0.5f), Random.Range(2f,2.5f), Random.Range(0f,0.5f));
            DamagePopUpGenerator.current.DamagePopup(transform.position + transform.forward+ randomPopUp, amount.ToString(), Color.yellow);
            // if (boss1Behaviour != null && boss1Behaviour.damageTimer != 0)
            // {
            //     boss1Behaviour.damageTaken += amount;
            // }
            healthSlider.value = enemyHealth;
            if (enemyHealth <= 0)
            {
                healOrbsLoot.GenerateHealOrbs();
                Destroy(gameObject);
            }
        }
        else
        {
            amount = 0;
            Vector3 randomPopUp = new Vector3(Random.Range(0f,0.25f), Random.Range(0.25f,0.5f), Random.Range(0f,0.25f));
            DamagePopUpGenerator.current.DamagePopup(transform.position + randomPopUp, amount.ToString(), Color.yellow);
            return;
        }
    }
    public void Invulnerable()
    {
        isInvulnerable = true;
    }
    public void Vulnerable()
    {
        isInvulnerable = false;
    }
}
