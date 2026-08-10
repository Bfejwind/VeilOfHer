using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class EnemyHP : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Boss1Behaviour boss1Behaviour;
    public float enemyHealth,enemyHealthWidth,enemyHealthHeight;
    public float enemyMaxHealth = 100f;
    public float damageAmt;
    public Slider healthSlider;
    private float sliderFillDuration = 0.5f;
    public bool shotAt;
    private HealOrbsLoot healOrbsLoot;
    public bool isInvulnerable;
    [SerializeField] private GameObject dissolveVFX;
    [SerializeField] private float dissolveDelay;
    [SerializeField] private GameObject explodeVFX;
    [SerializeField] private float explodeDelay;
    [SerializeField] private GameObject model;
    [SerializeField] private Collider[] objCollider;
    public bool isDed;
    [SerializeField] private VisualEffect onHitVFX;
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip deathSFX;
    [SerializeField] private AudioClip bossDeathSFX;
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
    private void Update()
    {
        FillSliderGradual();
    }
    public void TakingDamage(float amount)
    {
        if (!isInvulnerable)
        {
            shotAt = true;
            enemyHealth -= amount;
            UpdateHealthSlider();
            OnHitEffect();
            DamagePopUp(amount);
            if (enemyHealth <= 0)
            {
                isDed = true;
                healOrbsLoot.GenerateHealOrbs();
                StartCoroutine(EnemyDeath());
            }
        }
        else
        {
            amount = 0;
            DamagePopUp(amount);
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
    private void UpdateHealthSlider()
    {
        enemyHealth = Mathf.Clamp(enemyHealth, 0f, enemyMaxHealth);
        healthSlider.value = enemyHealth;
    }
    private void FillSliderGradual()
    {
        float speed = Mathf.Abs(enemyHealth - healthSlider.value) / sliderFillDuration;
        healthSlider.value = Mathf.MoveTowards(healthSlider.value, enemyHealth, speed * Time.deltaTime);
    }
    private IEnumerator EnemyDeath()
    {
        audioSource.PlayOneShot(deathSFX);
        foreach (Collider collider in objCollider)
        {
            collider.enabled = false;
        }
        dissolveVFX.SetActive(true);
        yield return new WaitForSeconds(dissolveDelay);
        if (boss1Behaviour != null)
        {
            audioSource.PlayOneShot(bossDeathSFX);
        }
        explodeVFX.SetActive(true);
        model.SetActive(false);
        yield return new WaitForSeconds(explodeDelay);
        Destroy(gameObject);
    }
    private void OnHitEffect()
    {
        onHitVFX.SendEvent("PlayVFX");
    }
    private void DamagePopUp(float amount)
    {
        Vector3 randomPopUp = new Vector3(Random.Range(0f,0.5f), Random.Range(2f,2.5f), Random.Range(0f,0.5f));
        DamagePopUpGenerator.current.DamagePopup(transform.position + transform.forward+ randomPopUp, amount.ToString(), Color.yellow);
    }
}
