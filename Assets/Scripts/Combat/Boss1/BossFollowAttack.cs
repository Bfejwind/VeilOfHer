using Unity.VisualScripting;
using UnityEngine;

public class BossFollowAttack : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private float damage = 5.0f;
    [SerializeField] private Transform playerTarget;
    [SerializeField] private float speed;
    [SerializeField] private float stopFollowDistance = 2.0f;
    private Vector3 movementDirection;
    private bool followingPlayer = true;
    [SerializeField] private GameObject boss;
    private Boss1Behaviour boss1Behaviour;
    [Header("Audio")]
    [SerializeField] public AudioSource audioSource;
    [SerializeField] private AudioClip followSFX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (playerTarget == null)
        {
            playerTarget = GameObject.Find("aimTarget").transform;
        }
        if (boss == null)
        {
            boss = GameObject.Find("Boss1");
        }
        boss1Behaviour = boss.GetComponent<Boss1Behaviour>();
        
    }
    void Start()
    {
        audioSource.PlayOneShot(followSFX);
        speed = boss1Behaviour.followAttackVelocity;
        duration = boss1Behaviour.followAttackDuration;
        Destroy(gameObject, duration);
    }
    void Update()
    {
        if (followingPlayer)
        {
            Vector3 directionToPlayer = (playerTarget.position - transform.position).normalized;
            movementDirection = directionToPlayer;
            float distance = Vector3.Distance(transform.position, playerTarget.position);
            transform.position = Vector3.MoveTowards(transform.position, playerTarget.position, speed * Time.deltaTime);
            if (distance <= stopFollowDistance)
            {
                followingPlayer = false;
            }
        }
        else
        {
            transform.position += movementDirection * speed * Time.deltaTime;
            
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out LaserBehaviour laserState))
        {
            if (laserState.laserOn && !other.CompareTag("RockLaser"))
            {
                //print("hit" + other.gameObject.name);
                Destroy(gameObject);
            }
            else
            {
                return;
            }
        }
        if (other.TryGetComponent(out PlayerHealth playerHP))
        {
            if (playerHP.IsInvulnerable)
            {
                Destroy(gameObject);
            }
            else
            {
                playerHP.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        else if (other.gameObject.CompareTag("Environment"))
        {
            //print("hit" + other.gameObject.name);
            Destroy(gameObject);
        }
    }
}
