using UnityEngine;
using UnityEngine.XR;

public class BossStateMachine : MonoBehaviour
{
    
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
    private void Start()
    {
        ChangeState(BossStates.Idle);
    }
    private void ChangeState(BossStates newState)
    {
        CurrentState = newState;
    }
}
