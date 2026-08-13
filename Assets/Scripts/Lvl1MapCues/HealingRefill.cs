using System.Collections;
using UnityEngine;

public class HealingRefill : MonoBehaviour
{
    private Coroutine refillCoroutine;
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth playerHp))
        {
            if (refillCoroutine == null)
            {
                refillCoroutine = StartCoroutine(RefillInterval(playerHp));
            }

        }
        if (other.TryGetComponent(out Dash stamina))
        {
            stamina.RefillStamina();
        }
    }
    private IEnumerator RefillInterval(PlayerHealth playerHp)
    {
        Debug.Log("Refilled");
        playerHp.OnHeal();
        playerHp.RechargeHeal(playerHp.maxRecharge);
        yield return new WaitForSeconds(1.0f);
        refillCoroutine = null;
    }
}
