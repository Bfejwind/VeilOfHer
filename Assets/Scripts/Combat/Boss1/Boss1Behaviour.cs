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
    [Header("Difficulty Settings")]
    [SerializeField] private float damageTimer = 0f;
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
            damageTimer += Time.deltaTime;
            StartCoroutine(BossAOE());
            attackStarted = true;
        }
    }
    private void UpdateBossState()
    {
        
    }
    private IEnumerator BossAOE()
    {
        while (true)
        {
            if (isPlayerInRange)
            {
                //Debug.Log("Boss AOE Attack");
                GameObject bossAOE = Instantiate(bossAOEPrefab, firePoint.position, Quaternion.identity);
                bossAOE.GetComponent<Rigidbody>().AddForce(firePoint.forward.normalized * aoeAttackVelocity, ForceMode.Impulse);
            }
            else
            {
                //Debug.Log("Boss Follow Attack");
                GameObject followAttack = Instantiate(followAttackPrefab, firePoint.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(WaveAttackDelay);
            GameObject waveAttack = Instantiate(bossWavePrefab, firePoint.position, transform.rotation * Quaternion.Euler(90,0,0));
            waveAttack.GetComponent<Rigidbody>().AddForce(firePoint.forward.normalized * WaveAttackVelocity, ForceMode.Impulse);
            yield return new WaitForSeconds(SummonAttackDelay);
            //Shining Animation to show absorbing
            if (damageTimer <= damageTimerThreshold && damageTaken >= damageTakenThreshold)
            {
                GameObject summon1 = Instantiate(summonPrefab, summonPoint1.position, Quaternion.identity);
                GameObject summon2 = Instantiate(summonPrefab, summonPoint2.position, Quaternion.identity);
            }
            damageTimer = 0;
            damageTaken = 0;
            yield return new WaitForSeconds(abilityAttackDelay);
        }

    }

}
