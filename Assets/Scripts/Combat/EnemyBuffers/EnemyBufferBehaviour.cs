using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class EnemyBufferBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private Transform bossTransform;
    [SerializeField] private Transform channellingSpawnPoint;
    [SerializeField] private float channelDistance = 5f;
    private Vector3 distToBoss;
    private bool inChannelRange;
    private bool isChannelling;
    private float channellingTimer;
    [SerializeField] private float channellingSuccessTime = 5.0f;
    private bool buffApplied;
    [SerializeField] private ParticleSystem channellingEffect;
    [Header("Layers")]
    [SerializeField] private LayerMask terrainLayer;
    [Header("Ability Interactions")]
    public int controlEffects;
    public bool canMove => controlEffects == 0;
    public bool canAttack => controlEffects == 0;

    private void Awake()
    {
        if (navAgent == null)
        {
            navAgent = GetComponent<NavMeshAgent>();
        }
        if (bossTransform == null)
        {
            GameObject bossObj = GameObject.Find("Boss1");
            if (bossObj != null)
            {
                bossTransform = bossObj.transform;
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
            buffApplied = true;
            // Apply the buff to the boss here
            Debug.Log("Buff applied to the boss!");
            Destroy(gameObject);
        }
    }

    private void DetectBoss()
    {
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
            if (!channellingEffect.isPlaying)
            {
                isChannelling = true;
                channellingTimer += Time.deltaTime;
                channellingEffect.Play();
            }
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
}
