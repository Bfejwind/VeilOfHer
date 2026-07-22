using UnityEngine;

public class BossWaveAttack : MonoBehaviour
{
    [SerializeField] private float duration = 5.0f;
    [SerializeField] private float damage = 5.0f;
    void Start()
    {
        Destroy(gameObject, duration);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Environment"))
        {
            //print("hit" + other.gameObject.name);
            Destroy(gameObject);
        }
        if (other.TryGetComponent(out PlayerHealth playerHP))
        {
            //print("hit" + other.gameObject.name);
            playerHP.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
