using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class EnemyBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;
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
    [SerializeField] private float forwardShotForce = 10f;
    [SerializeField] private float verticalShotForce = 1.3f;

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
        if (enemyHP == null)
        {
            enemyHP = GetComponent<EnemyHP>();
        }
    }
    private void Update()
    {
        if (enemyHP.isDed)
        {
            return;
        }
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

    //Shooting
    private void FireProjectile()
    {
        if (projectilePrefab == null || firePoint == null) return;

        Rigidbody projectileRB = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        Vector3 aimDirection = (playerTransform.position - firePoint.position).normalized;
        projectileRB.AddForce(aimDirection * forwardShotForce, ForceMode.Impulse);
        projectileRB.AddForce(aimDirection * verticalShotForce, ForceMode.Impulse);

        Destroy(projectileRB.gameObject, 3.0f);
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

    private void PerformAttack()
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
            return;
        }

        if (!isOnAttackCooldown)
        {
            FireProjectile();
            StartCoroutine(AttackCooldownRoutine());
        }
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
        if (isPlayerVisible && isPlayerInRange)
        {
            PerformAttack();
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
            PerformAttack();
            return;
        }
    }
}
