using UnityEngine;

public class BossNormalAttack : MonoBehaviour
{
    [SerializeField] private float duration = 10.0f;
    [SerializeField] private float damage = 10.0f;
    // [Header("Audio")]
    // [SerializeField] public AudioSource waveHumSource;
    // [SerializeField] private AudioClip waveConstantSFX;
    void Start()
    {
        // waveHumSource.clip = waveConstantSFX;
        // waveHumSource.loop = true;
        // waveHumSource.Play();
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
