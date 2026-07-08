using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss1Behaviour : MonoBehaviour
{
    //Lasers
    //[SerializeField] private GameObject laserPrefab;
    //private Transform laserSpawnPoint;
    //References
    private EnemyBehaviour enemyBehaviour;
    private bool attackStarted;
    private enum BossState
    {
        Idle,
        NormalAttack,
        AOEAttack,

    }
    //AOE attack
    [SerializeField] private GameObject bossAOEPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float aoeAttackVelocity = 10f;
    private List<Transform> aoeSpawnPoints = new List<Transform>();
    //Summons
    [SerializeField] private GameObject summonPrefab;
    [SerializeField] private Transform summonPoint1;
    [SerializeField] private Transform summonPoint2;
    //Dodge
    private float dodgeCooldown = 30.0f;
    private bool dodgeAvailable = true;
    public bool incomingAttack = false;
    private void Awake()
    {
        if (firePoint == null)
        {
            firePoint = transform.Find("FirePoint");
        }
        if (enemyBehaviour == null)
        {
            enemyBehaviour = GetComponent<EnemyBehaviour>();
        }
    }
    private void Update()
    {
        if (!attackStarted &&enemyBehaviour.isPlayerVisible)
        {
            StartCoroutine(BossAOE());
            attackStarted = true;
        }
        if (dodgeAvailable && incomingAttack)
        {
            BossDodge();
        }
    }
    private IEnumerator BossAOE()
    {
        while (true)
        {
            Debug.Log("Boss AOE Attack");
            GameObject bossAOE = Instantiate(bossAOEPrefab, firePoint.position, Quaternion.identity);
            bossAOE.GetComponent<Rigidbody>().AddForce(firePoint.forward.normalized * aoeAttackVelocity, ForceMode.Impulse);
            yield return new WaitForSeconds(2.0f);
            GameObject summon1 = Instantiate(summonPrefab, summonPoint1.position, Quaternion.identity);
            GameObject summon2 = Instantiate(summonPrefab, summonPoint2.position, Quaternion.identity);
            yield return new WaitForSeconds(5.0f);
        }

    }

    private void BossDodge()
    {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position, transform.right, out hit, 5f))
        {
            
        }

        dodgeAvailable = false;
        incomingAttack = false;
        StartCoroutine(ResetDodge());
    }
    // public static int WrapValue(int value, int min, int max)
    // {
    //     int range = max - min + 1; //min cant be 0
    //     int wrappedValue = (value - min) % range;
    // }
    private IEnumerator ResetDodge()
    {
        yield return new WaitForSeconds(dodgeCooldown);
        dodgeAvailable = true;
    }
}
