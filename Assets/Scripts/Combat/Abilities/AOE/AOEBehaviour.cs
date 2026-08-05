using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class AOEBehaviour : MonoBehaviour
{
    private float aoeAppliedDamage;
    public float aoeUpgradeDamage;
    private bool hasHit;
    private List<EnemyBehaviour>aoeAffectedEnemies = new();

    public float AOEDamageCalc(float damageAmount)
    {
        aoeAppliedDamage = Mathf.RoundToInt(UnityEngine.Random.Range(damageAmount -5,damageAmount + 5));
        aoeAppliedDamage = Mathf.Max(aoeAppliedDamage,0);
        return aoeAppliedDamage;
    }
    void OnTriggerStay(Collider other)
    {
        if (hasHit)
        {
            return;
        }
        //Debug.Log("hit:" + other.gameObject.name);
        if (other.TryGetComponent(out EnemyBehaviour enemy))
        {
            if (!aoeAffectedEnemies.Contains(enemy))
            {
                aoeAffectedEnemies.Add(enemy);
                if (other.TryGetComponent(out EnemyHP enemyHealth))
                {
                    hasHit = true;
                    enemyHealth.TakingDamage(aoeAppliedDamage);
                }
                ;
            }
        }
    }
}
