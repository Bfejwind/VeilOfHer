using Unity.VisualScripting;
using UnityEngine;

public class BossUltraInstinct : MonoBehaviour
{
    private Boss1Behaviour bossBehaviour;
    private void Awake()
    {
        bossBehaviour = GetComponent<Boss1Behaviour>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            // Trigger the dodge behavior
            if (bossBehaviour != null)
            {
                bossBehaviour.incomingAttack = true;
            }
        }
    }
}
