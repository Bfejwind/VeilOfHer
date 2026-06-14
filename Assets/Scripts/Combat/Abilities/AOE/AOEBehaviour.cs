using UnityEngine;

public class AOEBehaviour : MonoBehaviour
{
    private float aoeAppliedDamage;
    public float aoeUpgradeDamage;

    public float AOEDamageCalc(float damageAmount)
    {
        aoeAppliedDamage = damageAmount;
        return aoeAppliedDamage;
    }
    void OnTriggerStay(Collider other)
    {
        Debug.Log("hit:" + other.gameObject.name);
        if (other.TryGetComponent(out EnemyHP enemy))
        {
            enemy.TakingDamage(aoeAppliedDamage);
        }
    }
}
