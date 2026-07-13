using UnityEngine;

public class BossAOEBehaviour : MonoBehaviour
{
    [SerializeField] private float duration = 5.0f;
    [SerializeField] private float damage = 5.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, duration);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth playerHP))
        {
            //print("hit" + other.gameObject.name);
            playerHP.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

}
