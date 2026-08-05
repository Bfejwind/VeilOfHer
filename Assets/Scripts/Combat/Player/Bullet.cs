using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage;
    private bool hasHit;
    //[Header("Layers")]
    public void DamageCalculation(float damageAmount)
    {
        damage = Mathf.RoundToInt(UnityEngine.Random.Range(damageAmount -5,damageAmount + 5));
        damage = Mathf.Max(damage,0);
    }
    void OnTriggerEnter(Collider other)
    {
        if (hasHit)
        {
            return;
        }
        if (other.TryGetComponent(out EnemyHP enemy))
        {
            //print("Bullet hit" + other.gameObject.name);
            //GameManager.Instance.ChangeLightRadius(2.0f);
            hasHit = true;
            enemy.TakingDamage(damage);
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Environment"))
        {
            //print("hit" + other.gameObject.name);
            Destroy(gameObject);
        }
    }
}
