using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class AOEBehaviour : MonoBehaviour
{
    private float aoeAppliedDamage;
    public float aoeUpgradeDamage;
    private List<EnemyBehaviour>aoeAffectedEnemies = new();

    public float AOEDamageCalc(float damageAmount)
    {
        aoeAppliedDamage = damageAmount;
        return aoeAppliedDamage;
    }
    void OnTriggerStay(Collider other)
    {
        //Debug.Log("hit:" + other.gameObject.name);
        if (other.TryGetComponent(out EnemyBehaviour enemy))
        {
            if (!aoeAffectedEnemies.Contains(enemy))
            {
                aoeAffectedEnemies.Add(enemy);
                if (other.TryGetComponent(out EnemyHP enemyHealth))
                {
                    enemyHealth.TakingDamage(aoeAppliedDamage);
                }
                ;
            }
        }
    }
}
