using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float damage = 20.0f;
    //[Header("Layers")]
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
            if (playerHP.IsInvulnerable)
            {
                return;
            }
            else
            {
                playerHP.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
