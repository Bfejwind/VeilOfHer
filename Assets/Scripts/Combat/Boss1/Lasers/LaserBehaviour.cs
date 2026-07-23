using System.Collections;
using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{
    [SerializeField] private float damage = 20.0f;
    [SerializeField] private GameObject laserHolder;
    [SerializeField] private float laserDownTime = 10.0f;
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
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("DisableLaser"))
        {
            laserHolder.SetActive(false);
        }
    }
    private IEnumerator DeactivateLaser()
    {
        //Laser down SFX
        laserHolder.SetActive(false);
        yield return new WaitForSeconds(laserDownTime);
        //Laser Up SFX
        laserHolder.SetActive(true);
    }
}
