using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform warningOrigin;
    [SerializeField] private GameObject warningPrefab;
    private Rigidbody rb;
    private EnemyHP enemyHP;

    [Header("Layers")]
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private LayerMask playerLayerMask;

    [Header("Patrol Settings")]
    [SerializeField] private float patrolRadius = 10f;
    private Vector3 currentPatrolPoint;
    private bool hasPatrolPoint;

    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 1f;
    private bool isOnAttackCooldown;

    [Header("Warning Settings")]
    private bool warned;

    [Header("Detection Ranges")]
    [SerializeField] private float visionRange = 20f;
    [SerializeField] private float attackRange = 10f;
    public bool isPlayerVisible;
    public bool isPlayerInRange;

    [Header("Ability Interactions")]
    public int controlEffects;
    public bool canMove => controlEffects == 0;
    public bool canAttack => controlEffects == 0;

    private void Awake()
    {
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.Find("aimTarget");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
        }
        if (navAgent == null)
        {
            navAgent = GetComponent<NavMeshAgent>();
        }
        if (warningOrigin == null)
        {
            warningOrigin = transform.Find("WarningRaycastOrigin");
        }
        if (enemyHP == null)
        {
            enemyHP = GetComponent<EnemyHP>();
        }
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        Debug.DrawRay(warningOrigin.position, Vector3.down * 20f, Color.red);
        DetectPlayer();
        if (!enemyHP.shotAt)
        {
            UpdateBehaviourState();
        }
        else
        {
            EnragedBehaviourState();
        }
    }

    //See vision and Attack range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);
    }

    //Detects player
    private void DetectPlayer()
    {
        isPlayerVisible = Physics.CheckSphere(transform.position, visionRange, playerLayerMask);
        isPlayerInRange = Physics.CheckSphere(transform.position, attackRange, playerLayerMask);
    }


    //Patrolling setup
    private void FindPatrolPoint()
    {
        float randomX = Random.Range(-patrolRadius, patrolRadius);
        float randomZ = Random.Range(-patrolRadius, patrolRadius);

        Vector3 potentialPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(potentialPoint, -transform.up, 2f, terrainLayer))
        {
            currentPatrolPoint = potentialPoint;
            hasPatrolPoint = true;
        }
    }

    //Fire rate
    private IEnumerator AttackCooldownRoutine()
    {
        warned = false;
        isOnAttackCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        isOnAttackCooldown = false;
    }

    //Patrolling
    private void PerformPatrol()
    {
        if (!hasPatrolPoint)
        {
            //Debug.Log("Setting Point");
            FindPatrolPoint();
        }
        if (hasPatrolPoint)
        {
            //Debug.Log("Moving to point");
            navAgent.SetDestination(currentPatrolPoint);
        }
        if (Vector3.Distance(transform.position, currentPatrolPoint) < 1f)
        {
            //Debug.Log("Point Reached");
            hasPatrolPoint = false;
        }
    }
    
    //Player Sighted
    private void PerformChase()
    {
        if (playerTransform != null)
        {
            navAgent.SetDestination(playerTransform.position);
        }
    }

    //Player in Attack range

    private IEnumerator PerformWarning()
    {
        navAgent.velocity = Vector3.zero;
        navAgent.SetDestination(transform.position);

        if (playerTransform != null)
        {
            Vector3 target = playerTransform.position;
            target.y = transform.position.y;
            transform.LookAt(target);
        }
        if (!canAttack)
        {
            yield break;
        }

        if (!isOnAttackCooldown && !warned)
        {
            if (Physics.Raycast(warningOrigin.position, Vector3.down, out RaycastHit hit, 3.0f, terrainLayer))
            {
                Vector3 warningPosition = hit.point + Vector3.up * 0.05f;
                Instantiate(warningPrefab, warningPosition, transform.rotation);
                //PreCharge Animation
                warned = true;
                yield return new WaitForSeconds(2f);
                PerformCharge();
                StartCoroutine(AttackCooldownRoutine());
            }
            //Attack script
            else
            {
                Debug.Log("ray not hitting ground");
            }
        }
        else
        {
            //Play tired animation
        }
    }
    private void PerformCharge()
    {
        Debug.Log("Charged");
        rb.AddForce(transform.forward, ForceMode.VelocityChange);
    }
    private void UpdateBehaviourState()
    {
        if (controlEffects > 0)
        {
            UpdateMovementState();
            return;
        }
        if (!isPlayerVisible && !isPlayerInRange)
        {
            PerformPatrol();
            return;
        }
        if (isPlayerVisible && !isPlayerInRange)
        {
            PerformChase();
            return;
        }
        if (isPlayerVisible && isPlayerInRange && !warned)
        {
            StartCoroutine(PerformWarning());
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
    private void EnragedBehaviourState()
    {
        if (controlEffects > 0)
        {
            UpdateMovementState();
            return;
        }
        if (!isPlayerInRange)
        {
            PerformChase();
            return;
        }
        if (isPlayerVisible && isPlayerInRange)
        {
            StartCoroutine(PerformWarning());
            return;
        }
    }
}
