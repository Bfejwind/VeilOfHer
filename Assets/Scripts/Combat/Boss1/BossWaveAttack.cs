using UnityEngine;

public class BossWaveAttack : MonoBehaviour
{
    [SerializeField] private float duration = 5.0f;
    [SerializeField] private float damage = 5.0f;
    [Header("Audio")]
    [SerializeField] public AudioSource waveHumSource;
    [SerializeField] private AudioClip waveConstantSFX;
    void Start()
    {
        waveHumSource.clip = waveConstantSFX;
        waveHumSource.loop = true;
        waveHumSource.Play();
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
