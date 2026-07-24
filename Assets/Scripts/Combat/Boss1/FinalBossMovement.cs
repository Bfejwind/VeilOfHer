using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using AmplifyShaderEditor;

public class FinalBossMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private Transform playerTransform;
    [Header("Positions")]
    [SerializeField] private List<Transform>BossPositions = new List<Transform>();
    [SerializeField] private int currentPosition = 0;
    private Dictionary<Transform,int> teleportDict = new(); 
    [Header("Teleport Settings")]
    private float teleportTimer = 0;
    [SerializeField] private float teleportInterval = 5.0f;
    public bool canTeleport => controlEffects == 0;
    public bool isTeleporting;
    [SerializeField] private FinalBossAudio bossAudio;

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
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.Find("aimTarget");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
        }
        if (bossAudio == null)
        {
            bossAudio = GetComponent<FinalBossAudio>();
        }
    }
    private void Update()
    {
        UpdateBehaviourState();
        teleportTimer += Time.deltaTime;
        if (teleportTimer >= teleportInterval && canTeleport && !isTeleporting)
        {
            isTeleporting = true;
            teleportTimer = 0;
            currentPosition ++;
            StartCoroutine(TeleportBoss(WrapNum(currentPosition, BossPositions.Count)));
            isTeleporting = false;
        }
    }
    private void UpdateBehaviourState()
    {
        if (controlEffects > 0)
        {
            UpdateMovementState();
            return;
        }
        
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
    private int WrapNum(int value, int max)
    {
        return (value %  max + max) % max;
    }
    private IEnumerator TeleportBoss(int posNum)
    {
        bossAudio.PlayTeleportOut();
        yield return new WaitForSeconds(0.2f);
        transform.position = BossPositions[posNum].position;
        bossAudio.PlayTeleportIn();
    }
}
