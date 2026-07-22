using UnityEngine;
using UnityEngine.XR;

public class BossStateMachine : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private BossMovement bossMovementScript;
    public enum BossStates
    {
        Idle,
        Chase,
        Attack,
        Recover,
        Stunned,
        Dead
    }
    public BossStates CurrentState { get; private set; }
    private void Awake()
    {
        if (player == null)
        {
            player = GameObject.Find("Player").transform;
        }
        if (bossMovementScript == null)
        {
            bossMovementScript = transform.GetComponent<BossMovement>();
        }
    }
    private void Start()
    {
        ChangeState(BossStates.Idle);
    }
    private void ChangeState(BossStates newState)
    {
        CurrentState = newState;
    }
    private void ChaseActivate()
    {
        bossMovementScript.ChasePlayer(player);
    }
}
