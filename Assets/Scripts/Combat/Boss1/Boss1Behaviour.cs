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
    }
    private IEnumerator BossAOE()
    {
        while (true)
        {
            Debug.Log("Boss AOE Attack");
            yield return new WaitForSeconds(2.0f);
            GameObject bossAOE = Instantiate(bossAOEPrefab, firePoint.position, Quaternion.identity);
            bossAOE.GetComponent<Rigidbody>().AddForce(firePoint.forward.normalized * aoeAttackVelocity, ForceMode.Impulse);
            GameObject summon1 = Instantiate(summonPrefab, summonPoint1.position, Quaternion.identity);
            GameObject summon2 = Instantiate(summonPrefab, summonPoint2.position, Quaternion.identity);
            yield return new WaitForSeconds(5.0f);
        }

    }
}
