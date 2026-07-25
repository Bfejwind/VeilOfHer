using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;
public class EnemyBufferBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private Boss1Behaviour boss1Behaviour;
    [SerializeField] private Transform bossTransform;
    [SerializeField] private Transform channellingSpawnPoint;
    [SerializeField] private float channelDistance = 5f;
    private Vector3 distToBoss;
    private bool inChannelRange;
    private bool isChannelling;
    private float channellingTimer;
    [SerializeField] private float channellingSuccessTime = 5.0f;
    [Header("Buff Settings")]
    private bool buffApplied;
    [SerializeField] private float buffDuration = 10.0f;
    [SerializeField] private float buffEffect = 2.0f;
    [SerializeField] private VisualEffect channellingEffect;
    [Header("Layers")]
    [SerializeField] private LayerMask terrainLayer;
    [Header("Ability Interactions")]
    public int controlEffects;
    public bool canMove => controlEffects == 0;
    public bool canAttack => controlEffects == 0;

    private void Awake()
    {
        channellingEffect = GetComponentInChildren<VisualEffect>();
        //channellingEffect.Stop();
        if (navAgent == null)
        {
            navAgent = GetComponent<NavMeshAgent>();
        }
        if (boss1Behaviour == null)
        {
            GameObject bossObj = GameObject.Find("Boss1");
            if (bossObj != null)
            {
                boss1Behaviour = bossObj.GetComponent<Boss1Behaviour>();
            }
        }
        if (bossTransform == null)
        {
            GameObject bossObj = GameObject.Find("Boss1");
            if (bossObj != null)
            {
                bossTransform = bossObj.transform;
            }
            else
            {
                DestroySelf();
            }
        }
        if (channellingSpawnPoint == null)
        {
            channellingSpawnPoint = transform.Find("ChannellingSpawnPoint");
        }

    }
    private void Update()
    {
        if (!buffApplied)
        {
            DetectBoss();
            UpdateBehaviourState();
        }
        if (isChannelling && channellingTimer >= channellingSuccessTime)
        {
            Debug.Log("Channelling complete");
            buffApplied = true;
            channellingEffect.SetFloat("spawnRate", 0f);
            // Apply the buff to the boss here
            boss1Behaviour.ApplyAttackSpeedBuff(buffDuration,buffEffect);
            Debug.Log("Buff applied to the boss!");
            DestroySelf();
        }
    }

    private void DetectBoss()
    {
        if (bossTransform == null)
        {
            DestroySelf();
            return;
        }
        distToBoss = bossTransform.position - transform.position;
        inChannelRange = distToBoss.magnitude <= channelDistance;
    }
    private void UpdateBehaviourState()
    {
        if (controlEffects > 0)
        {
            UpdateMovementState();
            return;
        }
        if (!inChannelRange)
        {
            ChaseBoss();
            return;
        }
        if (inChannelRange)
        {
            navAgent.SetDestination(transform.position);
            transform.LookAt(bossTransform.position);
            isChannelling = true;
            boss1Behaviour.channelledUpon = true;
            channellingTimer += Time.deltaTime;
            channellingEffect.SetFloat("spawnRate", 16f);
            return;
        }
    }
    private void ChaseBoss()
    {
        navAgent.SetDestination(bossTransform.position);
    }
    public void ApplyControl()
    {
        controlEffects++;
        UpdateMovementState();
    }
    public void RemoveControl()
    {
        controlEffects = Mathf.Max(0, controlEffects-1);
        UpdateMovementState();
    }
    private void UpdateMovementState()
    {
        navAgent.SetDestination(transform.position);
        navAgent.isStopped = !canMove;
        navAgent.velocity = Vector3.zero;
    }
    private void DestroySelf()
    {
        boss1Behaviour.channelledUpon = false;
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        boss1Behaviour.channelledUpon = false;
    }
}
