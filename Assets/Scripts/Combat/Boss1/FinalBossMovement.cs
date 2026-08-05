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
    [SerializeField] private Boss1Behaviour boss1Behaviour;
    [Header("Positions")]
    [SerializeField] private List<Transform>BossPositions = new List<Transform>();
    [SerializeField] private int currentPosition = 0;
    private Dictionary<Transform,int> teleportDict = new(); 
    [Header("Teleport Settings")]
    private float teleportTimer = 0;
    [SerializeField] private float teleportInterval = 5.0f;
    public bool canTeleport => controlEffects == 0;
    public bool isTeleporting;
    [SerializeField] private GameObject teleportVFX;
    [SerializeField] private FinalBossAudio bossAudio;

    [Header("Ability Interactions")]
    public int controlEffects;
    public bool canMove => controlEffects == 0;
    public bool canAttack => controlEffects == 0;
    public float nerfDuration;
    [SerializeField] private float nerfEffect = 2.0f;

    private void Awake()
    {
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
    private void Start()
    {
        transform.position = BossPositions[0].position;
        teleportVFX.SetActive(false);
    }
    private void Update()
    {
        UpdateBehaviourState();
        teleportTimer += Time.deltaTime;
        if (teleportTimer >= teleportInterval && canTeleport && !isTeleporting && !boss1Behaviour.channelledUpon)
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
            // UpdateMovementState();
            return;
        }
        
    }
    public void ApplyControl()
    {
        controlEffects++;
    }
    public void RemoveControl()
    {
        controlEffects = Mathf.Max(0, controlEffects-1);
    }
    // private void UpdateMovementState()
    // {
    //     boss1Behaviour.ApplyAttackSpeedNerf(nerfDuration, nerfEffect);
    // }
    private int WrapNum(int value, int max)
    {
        return (value %  max + max) % max;
    }
    private IEnumerator TeleportBoss(int posNum)
    {
        teleportVFX.SetActive(true);
        bossAudio.PlayTeleportOut();
        yield return new WaitForSeconds(2f);
        transform.position = BossPositions[posNum].position;
        teleportVFX.SetActive(false);
        bossAudio.PlayTeleportIn();
    }
}
