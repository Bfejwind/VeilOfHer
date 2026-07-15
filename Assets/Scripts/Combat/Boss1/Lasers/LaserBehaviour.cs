using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{
    [SerializeField] private float damage = 20.0f;
    //[Header("Layers")]
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth playerHP))
        {
            if (playerHP.IsInvulnerable)
            {
                return;
            }
            else
            {
                playerHP.TakeDamage(damage);
            }
        }
    }
}
