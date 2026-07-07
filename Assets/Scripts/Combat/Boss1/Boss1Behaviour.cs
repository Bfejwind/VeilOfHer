using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using UnityEngine;

public class Boss1Behaviour : MonoBehaviour
{
    //Lasers
    [SerializeField] private GameObject laserPrefab;
    private Transform laserSpawnPoint;
    private enum BossState
    {
        Idle,
        NormalAttack,
        AOEAttack,

    }
    //AOE attack
    [SerializeField] private GameObject bossAOEWarningPrefab;
    private List<Transform> aoeSpawnPoints = new List<Transform>();
    private void Start()
    {
        StartCoroutine(BossAOE());
    }
    private IEnumerator BossAOE()
    {
        while (true)
        {
            Debug.Log("Boss AOE Attack");
            yield return new WaitForSeconds(2.0f);
            Vector3 randomSpawnPoint = new Vector3(transform.position.x + Random.Range(-10f, 11f), 0f,transform.position.z + Random.Range(-10f, 11f));
            Instantiate(bossAOEWarningPrefab, randomSpawnPoint, Quaternion.identity);
        }

    }
}
