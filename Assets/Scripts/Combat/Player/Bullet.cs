using System.Runtime.InteropServices;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage;
    //[Header("Layers")]
    public void DamageCalculation(float damageAmount)
    {
        damage = damageAmount;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyHP enemy))
        {
            //print("hit" + other.gameObject.name);
            //GameManager.Instance.ChangeLightRadius(2.0f);
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
