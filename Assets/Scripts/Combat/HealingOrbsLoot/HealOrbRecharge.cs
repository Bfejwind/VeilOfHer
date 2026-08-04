using UnityEngine;

public class HealOrbRecharge : MonoBehaviour
{
    [SerializeField] private float rechargeAmt;
    [SerializeField] private float rechargeMin = 20f;
    [SerializeField] private float rechargeMax = 40f;
    private void Start()
    {
        rechargeAmt = Random.Range(rechargeMin, rechargeMax);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            if (playerHealth.currentRecharge < playerHealth.maxRecharge)
            {
                playerHealth.RechargeHeal(rechargeAmt);
                Destroy(gameObject);
            }
            else
            {
                return;
            }
        }
    }
}
