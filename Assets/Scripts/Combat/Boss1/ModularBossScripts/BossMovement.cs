using UnityEngine;
using UnityEngine.AI;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    public void ChasePlayer(Transform player)
    {
        agent.SetDestination(player.position);
    }
}
