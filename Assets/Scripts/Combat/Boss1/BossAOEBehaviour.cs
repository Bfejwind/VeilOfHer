using System.Collections;
using StarterAssets;
using UnityEngine;
using UnityEngine.VFX;

public class BossAOEBehaviour : MonoBehaviour
{
    [SerializeField] private float distance = 10.0f;
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float damage = 5.0f;
    [SerializeField] private float explosionRadius = 3.0f;
    [SerializeField] private float explosionForce = 4.0f;
    private Vector3 targetPosition;
    private bool isDissolving;
    private bool hitPlayer;
    [SerializeField] private ParticleSystem[] aoeShards;
    [SerializeField] private VisualEffect aoeTrail;
    [Header("Audio")]
    [SerializeField] public AudioSource shardSource;
    [SerializeField] public AudioSource trailSource;
    [SerializeField] private AudioClip bigShardsSFX;
    [SerializeField] private AudioClip trailSFX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trailSource.clip = trailSFX;
        trailSource.loop = true;
        trailSource.Play();
        targetPosition = transform.position + transform.forward * distance;
    }
    void Update()
    {
        if (transform.position == targetPosition || hitPlayer)
        {
            if (!isDissolving)
            {
                isDissolving = true;
                StartCoroutine(AOEShardBurst());
                //Dissolve anim
                //Off trigger collider
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
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
                hitPlayer = true;
                playerHP.TakeDamage(damage);
            }
        }
    }
    private IEnumerator AOEShardBurst()
    {
        //Audio
        

        Vector3 impactPoint = transform.position;
        Collider[] hits = Physics.OverlapSphere(impactPoint,explosionRadius);
        foreach (Collider hit in hits)
        {
            FirstPersonController player = hit.GetComponent<FirstPersonController>();
            if (player != null)
            {
                player.AddKnockback(impactPoint,explosionForce);
            }
        }
        foreach (ParticleSystem shards in aoeShards)
        {
            shards.Play();
        }
        shardSource.PlayOneShot(bigShardsSFX);
        yield return new WaitForSeconds(0.5f);
        aoeTrail.Stop();
        trailSource.Stop();
        yield return new WaitForSeconds(5.0f);
        Destroy(gameObject);
    }
}
