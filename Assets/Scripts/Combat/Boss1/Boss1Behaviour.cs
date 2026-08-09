using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;


public class Boss1Behaviour : MonoBehaviour
{
    //Lasers
    //[SerializeField] private GameObject laserPrefab;
    //private Transform laserSpawnPoint;
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private FinalBossMovement finalBossMovement;
    private EnemyHP bossHP;
    private bool attackStarted;
    [SerializeField] private float mini_AttackDelay = 0.5f;
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
    [Header("Boss Routine")]
    public bool laserRoutine1Started;
    public bool laserRoutine1Ended;
    private bool phase2RoutineStarted;
    public bool laserRoutine2Started;
    public bool laserRoutine2Ended;
    private bool phase3RoutineStarted;
    public bool laserArenaState;
    [Header("Follow Attack")]
    [SerializeField] private GameObject followAttackPrefab;
    [SerializeField] public float followAttackDuration = 15.0f;
    [SerializeField] public float followAttackVelocity = 10f;
    [Header("Knockback Attack")]
    [SerializeField] private GameObject bossAOEPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float aoeAttackVelocity = 10f;
    [Header("Wave Attack")]
    [SerializeField] private GameObject bossWavePrefab;
    [SerializeField] private float WaveAttackVelocity = 10f;
    [SerializeField] private float WaveAttackDelay = 1f;
    [Header("Normal Attack")]
    [SerializeField] private GameObject bossNormalAttackPrefab;
    [SerializeField] private float normalAttackVelocity = 10f;
    [Header("Laser Arena")]
    [SerializeField] private GameObject[] splineObjects;
    [SerializeField] private GameObject shieldEffect;
    private SplineAnimate[] splineAnimators;
    [Header("Summons")]
    [SerializeField] private Transform[] summonSpawnPos;
    [SerializeField] private GameObject realSummon;
    [SerializeField] private GameObject fakeSummon;
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
        if (bossHP == null)
        {
            bossHP = GetComponent<EnemyHP>();
        }
        if (finalBossMovement == null)
        {
            finalBossMovement = GetComponent<FinalBossMovement>();
        }
        splineAnimators = new SplineAnimate[splineObjects.Length];
        for (int i = 0; i < splineObjects.Length; i++)
        {
            splineAnimators[i] = splineObjects[i].GetComponent<SplineAnimate>();
        }
        for (int i = 0; i < splineAnimators.Length; i++)
        {
            SplineAnimate spline = splineAnimators[i];
            spline.Completed += () =>
            {
                spline.gameObject.SetActive(false);
            };
        }
    }
    private void Start()
    {
        foreach (GameObject splineObj in splineObjects)
        {
            splineObj.SetActive(false);
        }
        ShieldEffectOff();
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
        if (bossHP.isDed)
        {
            return;
        }
        DetectPlayer();
        if (playerTransform != null)
        {
            Vector3 target = playerTransform.position;
            target.y = transform.position.y;
            transform.LookAt(target);
        }
        if (!attackStarted && isPlayerVisible)
        {
            StartCoroutine(BossFightBegins());
            attackStarted = true;
        }
        if (bossHP.enemyHealth <= bossHP.enemyMaxHealth * 0.75f && !laserRoutine1Started)
        {
            StopCoroutine(Phase1Projectiles());
            StartCoroutine(LaserArena1());
            laserRoutine1Started = true;
        }
        if (laserRoutine1Ended && !phase2RoutineStarted)
        {
            StartCoroutine(Phase2projectiles());
            phase2RoutineStarted = true;
        }
        if (bossHP.enemyHealth <= bossHP.enemyMaxHealth * 0.25f && !laserRoutine2Started)
        {
            StopCoroutine(Phase2projectiles());
            StartCoroutine(LaserArena2());
            laserRoutine2Started = true;
        }
        if (laserRoutine2Ended && !phase3RoutineStarted)
        {
            StartCoroutine(Phase3Projectiles());
            phase3RoutineStarted = true;
        }
        if (bossHP.enemyHealth <= 0)
        {
            StopAllCoroutines();
        }
    }
    private IEnumerator BossFightBegins()
    {
        //Boss Music
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(Phase1Projectiles());
    }
    private IEnumerator Phase1Projectiles()
    {
        while (bossHP.enemyHealth > bossHP.enemyMaxHealth * 0.5f)
        {
            FollowAttack();
            yield return new WaitForSeconds(S_AttackDelay);
            FollowAttack();
            yield return new WaitForSeconds(S_AttackDelay);
            FollowAttack();
            yield return new WaitForSeconds(M_AttackDelay);
            KnockbackAttack();
            yield return new WaitForSeconds(S_AttackDelay);
            KnockbackAttack();
            yield return new WaitForSeconds(S_AttackDelay);
            KnockbackAttack();
            yield return new WaitForSeconds(M_AttackDelay);
            WaveSpreadAttack();
            yield return new WaitForSeconds(S_AttackDelay);
            FollowAttack();
            yield return new WaitForSeconds(S_AttackDelay);
            FollowAttack();
            yield return new WaitForSeconds(S_AttackDelay);
            FollowAttack();
            yield return new WaitForSeconds(M_AttackDelay);
            NormalSpreadAttack(0);
            yield return new WaitForSeconds(mini_AttackDelay);
            NormalSpreadAttack(10);
            yield return new WaitForSeconds(mini_AttackDelay);
            NormalSpreadAttack(20);
            yield return new WaitForSeconds(M_AttackDelay);
        }
    }
    private IEnumerator LaserArena1()
    {
        laserArenaState = true;
        bossHP.Invulnerable();
        ShieldEffectOn();
        SummonAttack();
        // splineObjects[0].SetActive(true);
        // splineAnimators[0].Play();
        yield return new WaitForSeconds(S_AttackDelay);
        // splineObjects[1].SetActive(true);
        // splineAnimators[1].Play();
        yield return new WaitForSeconds(S_AttackDelay);
        // splineObjects[2].SetActive(true);
        // splineAnimators[2].Play();
        yield return new WaitForSeconds(S_AttackDelay);
        // splineObjects[3].SetActive(true);
        // splineAnimators[3].Play();
        yield return new WaitForSeconds(L_AttackDelay);
        laserRoutine1Ended = true;
        bossHP.Vulnerable();
        ShieldEffectOff();
        laserArenaState = false;
    }
    private IEnumerator Phase2projectiles()
    {
        while (bossHP.enemyHealth > bossHP.enemyMaxHealth * 0.25f)
        {
            KnockbackAttack();
            yield return new WaitForSeconds(S_AttackDelay);
            FollowAttack();
            yield return new WaitForSeconds(S_AttackDelay);
            KnockbackAttack();
            yield return new WaitForSeconds(S_AttackDelay);
            FollowAttack();
            yield return new WaitForSeconds(S_AttackDelay);
            KnockbackAttack();
            yield return new WaitForSeconds(S_AttackDelay);
            FollowAttack();
            yield return new WaitForSeconds(M_AttackDelay);
            NormalSpreadAttack(0);
            yield return new WaitForSeconds(S_AttackDelay);
            NormalSpreadAttack(15);
            yield return new WaitForSeconds(S_AttackDelay);
            NormalSpreadAttack(25);
            yield return new WaitForSeconds(M_AttackDelay);
            WaveSpreadAttack();
            yield return new WaitForSeconds(S_AttackDelay);
            FollowAttack();
            yield return new WaitForSeconds(S_AttackDelay);
            KnockbackAttack();
            yield return new WaitForSeconds(S_AttackDelay);
            FollowAttack();
            yield return new WaitForSeconds(S_AttackDelay);
            KnockbackAttack();
            yield return new WaitForSeconds(S_AttackDelay);
            FollowAttack();
            yield return new WaitForSeconds(S_AttackDelay);
            KnockbackAttack();
            yield return new WaitForSeconds(M_AttackDelay);
        }
    }
    private IEnumerator LaserArena2()
    {
        laserArenaState = true;
        bossHP.Invulnerable();
        ShieldEffectOn();
        SummonIllusions();
        // splineObjects[0].SetActive(true);
        // splineAnimators[0].Restart(true);
        yield return new WaitForSeconds(S_AttackDelay);
        // splineObjects[1].SetActive(true);
        // splineAnimators[1].Restart(true);
        yield return new WaitForSeconds(S_AttackDelay);
        // splineObjects[2].SetActive(true);
        // splineAnimators[2].Restart(true);
        yield return new WaitForSeconds(S_AttackDelay);
        // splineObjects[3].SetActive(true);
        // splineAnimators[3].Restart(true);
        yield return new WaitForSeconds(L_AttackDelay);
        laserRoutine2Ended = true;
        bossHP.Vulnerable();
        ShieldEffectOff();
        laserArenaState = false;
    }
    private IEnumerator Phase3Projectiles()
    {
        while (bossHP.enemyHealth > 0)
        {
            FollowAttack();
            yield return new WaitForSeconds(S_AttackDelay);
            FollowAttack();
            yield return new WaitForSeconds(S_AttackDelay);
            FollowAttack();
            yield return new WaitForSeconds(S_AttackDelay);
            FollowAttack();
            yield return new WaitForSeconds(S_AttackDelay);
            FollowAttack();
            yield return new WaitForSeconds(M_AttackDelay);
            WaveSpreadAttack();
            yield return new WaitForSeconds(S_AttackDelay);
            WaveSpreadAttack();
            yield return new WaitForSeconds(S_AttackDelay);
            WaveSpreadAttack();
            yield return new WaitForSeconds(M_AttackDelay);
            NormalSpreadAttack(0);
            KnockbackRandomAttack();
            yield return new WaitForSeconds(S_AttackDelay);
            NormalSpreadAttack(5);
            KnockbackRandomAttack();
            yield return new WaitForSeconds(S_AttackDelay);
            NormalSpreadAttack(10);
            KnockbackRandomAttack();
            yield return new WaitForSeconds(M_AttackDelay);
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
    private void WaveSpreadAttack()
    {
        int projectileCount = 3;
        float spreadAngle = 30f;
        for (int i = 0; i < projectileCount; i++)
        {
            float angle = -spreadAngle/2 + (spreadAngle/(projectileCount - 1)) * i ;
            Quaternion rotation = firePoint.rotation * Quaternion.Euler(0,angle,0);
            GameObject waveAttack = Instantiate(bossWavePrefab, firePoint.position, rotation);
            waveAttack.GetComponent<Rigidbody>().AddForce(rotation * Vector3.forward * WaveAttackVelocity, ForceMode.Impulse);
        }
    }
    private void NormalSpreadAttack(float offsetAngle)
    {
        int projectileCount = 11;
        float spreadAngle = 120f;
        for (int i = 0; i < projectileCount; i++)
        {
            float angle = -spreadAngle/2 + ((spreadAngle/(projectileCount - 1)) * i) + offsetAngle;
            Quaternion rotation = firePoint.rotation * Quaternion.Euler(0,angle,0);
            GameObject normalAttack = Instantiate(bossNormalAttackPrefab, firePoint.position, rotation);
            normalAttack.GetComponent<Rigidbody>().AddForce(rotation * Vector3.forward * normalAttackVelocity, ForceMode.Impulse);
        }
    }
    private void KnockbackAttack()
    {
        Vector3 KBFirePoint = firePoint.position;
        KBFirePoint.y = 0f;
        GameObject bossAOE = Instantiate(bossAOEPrefab, KBFirePoint, transform.rotation);
    }
    private void KnockbackRandomAttack()
    {
        Vector3 KBFirePoint = firePoint.position;
        KBFirePoint.y = 0f;
        float randomAngle = Random.Range(0f, 120f);
        Quaternion randomRotation = firePoint.rotation * Quaternion.Euler(0, randomAngle, 0);
        GameObject bossAOE = Instantiate(bossAOEPrefab, KBFirePoint, randomRotation);
    }
    private void SummonAttack()
    {
        GameObject summon1 = Instantiate(summonPrefab, summonPoint1.position, Quaternion.identity);
        GameObject summon2 = Instantiate(summonPrefab, summonPoint2.position, Quaternion.identity);
    }
    private void SummonIllusions()
    {
        int realSpawn1 = Random.Range(0,summonSpawnPos.Length);
        int realSpawn2 = realSpawn1;
        while (realSpawn2 == realSpawn1)
        {
            realSpawn2 = Random.Range(0,summonSpawnPos.Length);
        }
        for (int i = 0; i < summonSpawnPos.Length; i++)
        {
            if (i == realSpawn1 || i == realSpawn2)
            {
                Instantiate(realSummon, summonSpawnPos[i].position, Quaternion.identity);
            }
            else
            {
                Instantiate(fakeSummon, summonSpawnPos[i].position, Quaternion.identity);
            }
        }
    }
    private void ShieldEffectOn()
    {
        shieldEffect.SetActive(true);
    }
    private void ShieldEffectOff()
    {
        shieldEffect.SetActive(false);
    }
    // private void Absorb()
    // {
    //     damageTimer += Time.deltaTime;
    //     if (damageTimer >= damageTimerThreshold && damageTaken < damageTakenThreshold)
    //     {
    //         ResetSummonTrackers();
    //         return;
    //     }
    //     else if (damageTimer >= damageTimerThreshold && damageTaken >= damageTakenThreshold)
    //     {
    //         SummonAttack();
    //         ResetSummonTrackers();
    //     }
    // }
    // private void ResetSummonTrackers()
    // {
    //     isAbsorbing = false;
    //     damageTimer = 0;
    //     damageTaken = 0;
    // }
    // public void ApplyAttackSpeedBuff(float duration, float effect)
    // {
    //     StartCoroutine(AttackSpeedBuff(duration,effect));
    // }
    // public IEnumerator AttackSpeedBuff(float duration, float effect)
    // {
    //     if (!isBuffed)
    //     {
    //         isBuffed = true;    
    //         WaveAttackDelay = Mathf.Max(0, WaveAttackDelay + effect);
    //         SummonAttackDelay = Mathf.Max(0, SummonAttackDelay + effect);
    //         abilityAttackDelay = Mathf.Max(0, abilityAttackDelay + effect);
    //         Debug.Log("AbilityDelay: " + abilityAttackDelay);
    //         Debug.Log("WaveDelay: " + WaveAttackDelay);
    //         yield return new WaitForSeconds(duration);
    //         WaveAttackDelay = WaveAttackDelay - effect;
    //         SummonAttackDelay = SummonAttackDelay - effect;
    //         abilityAttackDelay = abilityAttackDelay - effect;
    //         Debug.Log("AbilityDelayRestored: " + abilityAttackDelay);
    //         Debug.Log("WaveDelayResotred: " + WaveAttackDelay);
    //         isBuffed = false;
    //     }
    // }
    // public void ApplyAttackSpeedNerf(float duration, float effect)
    // {
    //     StartCoroutine(AttackSpeedNerf(duration,effect));
    // }
    // public IEnumerator AttackSpeedNerf(float duration, float effect)
    // {
    //     if (!isNerfed)
    //     {
    //         isNerfed = true;    
    //         WaveAttackDelay = Mathf.Max(0, WaveAttackDelay + effect);
    //         SummonAttackDelay = Mathf.Max(0, SummonAttackDelay + effect);
    //         abilityAttackDelay = Mathf.Max(0, abilityAttackDelay + effect);
    //         Debug.Log("AbilityDelay: " + abilityAttackDelay);
    //         Debug.Log("WaveDelay: " + WaveAttackDelay);
    //         yield return new WaitForSeconds(duration);
    //         WaveAttackDelay = WaveAttackDelay - effect;
    //         SummonAttackDelay = SummonAttackDelay - effect;
    //         abilityAttackDelay = abilityAttackDelay - effect;
    //         Debug.Log("AbilityDelayRestored: " + abilityAttackDelay);
    //         Debug.Log("WaveDelayResotred: " + WaveAttackDelay);
    //         isNerfed = false;
    //     }
    // }

}
