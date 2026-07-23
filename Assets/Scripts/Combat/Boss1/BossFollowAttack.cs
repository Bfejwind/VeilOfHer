using Unity.VisualScripting;
using UnityEngine;

public class BossFollowAttack : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private float damage = 5.0f;
    [SerializeField] private Transform playerTarget;
    [SerializeField] private float speed;
    [SerializeField] private GameObject boss;
    private Boss1Behaviour boss1Behaviour;
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
        speed = boss1Behaviour.followAttackVelocity;
        duration = boss1Behaviour.followAttackDuration;
        Destroy(gameObject, duration);
    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerTarget.position, speed * Time.deltaTime);
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
            //print("hit" + other.gameObject.name);
            playerHP.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("RockLaser"))
        {
            Destroy(gameObject);
        }
    }
}
