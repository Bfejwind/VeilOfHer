using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss1Behaviour : MonoBehaviour
{
    //Lasers
    //[SerializeField] private GameObject laserPrefab;
    //private Transform laserSpawnPoint;
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private FinalBossMovement finalBossMovement;
    private EnemyHP enemyHP;
    private bool attackStarted;
    [SerializeField] private float S_AttackDelay = 0.5f;
    [SerializeField] private float M_AttackDelay = 2f;
    [SerializeField] private float L_AttackDelay = 5f;
    private bool isAbsorbing;
    [Header("Layers")]
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private LayerMask playerLayerMask;

    [Header("Detection Ranges")]
    [SerializeField] private float visionRange = 20f;
    [SerializeField] private float attackRange = 10f;
    public bool isPlayerVisible;
    public bool isPlayerInRange;
    [SerializeField] private float abilityAttackDelay = 5.0f;
    [Header("Follow Attack")]
    [SerializeField] private GameObject followAttackPrefab;
    [SerializeField] public float followAttackDuration = 15.0f;
    [SerializeField] public float followAttackVelocity = 10f;
    [Header("AOE Attack")]
    [SerializeField] private GameObject bossAOEPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float aoeAttackVelocity = 10f;
    [Header("Wave Attack")]
    [SerializeField] private GameObject bossWavePrefab;
    [SerializeField] private float WaveAttackVelocity = 10f;
    [SerializeField] private float WaveAttackDelay = 1f;
    [Header("Summons")]
    [SerializeField] private GameObject summonPrefab;
    [SerializeField] private Transform summonPoint1;
    [SerializeField] private Transform summonPoint2;
    [SerializeField] private float SummonAttackDelay = 2f;
    private bool isBuffed;
    public bool channelledUpon;
    private bool isNerfed;
    [Header("Difficulty Settings")]
    [SerializeField] public float damageTimer = 0f;
    [SerializeField] private float damageTimerThreshold = 10.0f;
    public float damageTaken = 0f;
    [SerializeField] private float damageTakenThreshold = 50f;
    private void Awake()
    {
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.Find("aimTarget");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
        }
        if (firePoint == null)
        {
            firePoint = transform.Find("FirePoint");
        }
        if (enemyHP == null)
        {
            enemyHP = GetComponent<EnemyHP>();
        }
        if (finalBossMovement == null)
        {
            finalBossMovement = GetComponent<FinalBossMovement>();
        }
    }
    private void DetectPlayer()
    {
        isPlayerVisible = Physics.CheckSphere(transform.position, visionRange, playerLayerMask);
        isPlayerInRange = Physics.CheckSphere(transform.position, attackRange, playerLayerMask);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);
    }
    private void Update()
    {
        DetectPlayer();
        if (playerTransform != null)
        {
            Vector3 target = playerTransform.position;
            target.y = transform.position.y;
            transform.LookAt(target);
        }
        if (!attackStarted && isPlayerVisible)
        {
            StartCoroutine(BossAOE());
            attackStarted = true;
        }
        if (isAbsorbing)
        {
            Absorb();
        }
        //Absorption Timer
    }
    private IEnumerator BossAOE()
    {
        while (true)
        {
            if (isPlayerInRange)
            {
                //Debug.Log("Boss AOE Attack");
                Vector3 KBFirePoint = firePoint.position;
                KBFirePoint.y = 0f;
                GameObject bossAOE = Instantiate(bossAOEPrefab, KBFirePoint, transform.rotation);
                
            }
            else
            {
                //Debug.Log("Boss Follow Attack");
                GameObject followAttack = Instantiate(followAttackPrefab, firePoint.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(WaveAttackDelay);
            GameObject waveAttack = Instantiate(bossWavePrefab, firePoint.position, transform.rotation);
            waveAttack.GetComponent<Rigidbody>().AddForce(firePoint.forward.normalized * WaveAttackVelocity, ForceMode.Impulse);
            yield return new WaitForSeconds(SummonAttackDelay);
            //Shining Animation to show absorbing
            isAbsorbing = true;
            yield return new WaitForSeconds(abilityAttackDelay);
        }
    }
    private void FollowAttack()
    {
        GameObject followAttack = Instantiate(followAttackPrefab, firePoint.position, Quaternion.identity);
    }
    private void WaveAttack()
    {
        GameObject waveAttack = Instantiate(bossWavePrefab, firePoint.position, transform.rotation);
        waveAttack.GetComponent<Rigidbody>().AddForce(firePoint.forward.normalized * WaveAttackVelocity, ForceMode.Impulse);
    }
    private void KnockbackAttack()
    {
        Vector3 KBFirePoint = firePoint.position;
        KBFirePoint.y = 0f;
        GameObject bossAOE = Instantiate(bossAOEPrefab, KBFirePoint, transform.rotation);
        
    }
    private void Absorb()
    {
        damageTimer += Time.deltaTime;
        if (damageTimer >= damageTimerThreshold && damageTaken < damageTakenThreshold)
        {
            ResetSummonTrackers();
            return;
        }
        else if (damageTimer >= damageTimerThreshold && damageTaken >= damageTakenThreshold)
        {
            SummonAttack();
            ResetSummonTrackers();
        }
    }
    private void ResetSummonTrackers()
    {
        isAbsorbing = false;
        damageTimer = 0;
        damageTaken = 0;
    }
    private void SummonAttack()
    {
        GameObject summon1 = Instantiate(summonPrefab, summonPoint1.position, Quaternion.identity);
        GameObject summon2 = Instantiate(summonPrefab, summonPoint2.position, Quaternion.identity);
    }
    public void ApplyAttackSpeedBuff(float duration, float effect)
    {
        StartCoroutine(AttackSpeedBuff(duration,effect));
    }
    public IEnumerator AttackSpeedBuff(float duration, float effect)
    {
        if (!isBuffed)
        {
            isBuffed = true;    
            WaveAttackDelay = Mathf.Max(0, WaveAttackDelay + effect);
            SummonAttackDelay = Mathf.Max(0, SummonAttackDelay + effect);
            abilityAttackDelay = Mathf.Max(0, abilityAttackDelay + effect);
            Debug.Log("AbilityDelay: " + abilityAttackDelay);
            Debug.Log("WaveDelay: " + WaveAttackDelay);
            yield return new WaitForSeconds(duration);
            WaveAttackDelay = WaveAttackDelay - effect;
            SummonAttackDelay = SummonAttackDelay - effect;
            abilityAttackDelay = abilityAttackDelay - effect;
            Debug.Log("AbilityDelayRestored: " + abilityAttackDelay);
            Debug.Log("WaveDelayResotred: " + WaveAttackDelay);
            isBuffed = false;
        }
    }
    public void ApplyAttackSpeedNerf(float duration, float effect)
    {
        StartCoroutine(AttackSpeedNerf(duration,effect));
    }
    public IEnumerator AttackSpeedNerf(float duration, float effect)
    {
        if (!isNerfed)
        {
            isNerfed = true;    
            WaveAttackDelay = Mathf.Max(0, WaveAttackDelay + effect);
            SummonAttackDelay = Mathf.Max(0, SummonAttackDelay + effect);
            abilityAttackDelay = Mathf.Max(0, abilityAttackDelay + effect);
            Debug.Log("AbilityDelay: " + abilityAttackDelay);
            Debug.Log("WaveDelay: " + WaveAttackDelay);
            yield return new WaitForSeconds(duration);
            WaveAttackDelay = WaveAttackDelay - effect;
            SummonAttackDelay = SummonAttackDelay - effect;
            abilityAttackDelay = abilityAttackDelay - effect;
            Debug.Log("AbilityDelayRestored: " + abilityAttackDelay);
            Debug.Log("WaveDelayResotred: " + WaveAttackDelay);
            isNerfed = false;
        }
    }

}
