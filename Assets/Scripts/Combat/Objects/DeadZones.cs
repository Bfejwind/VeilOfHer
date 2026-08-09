using UnityEngine;

public class DeadZones : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth playerHp))
        {
            playerHp.TakeDamage(playerHp.playerMaxHealth);
        }
    }
}
