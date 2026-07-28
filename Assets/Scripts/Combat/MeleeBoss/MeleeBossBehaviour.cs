using UnityEngine;
using UnityEngine.AI;
using StarterAssets;
using System.Collections;
public class MeleeBossBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform warningOrigin;
    [SerializeField] private GameObject warningPrefab;
    private Rigidbody rb;
    [SerializeField] private PlayerHealth playerHP;
    private EnemyHP enemyHP;

    [Header("Layers")]
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private LayerMask playerLayerMask;

    [Header("Patrol Settings")]
    [SerializeField] private float patrolRadius = 10f;
    private Vector3 currentPatrolPoint;
    private bool hasPatrolPoint;

    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 0.2f;
    private bool isOnAttackCooldown;
    [SerializeField] private float chargeSpeed = 30.0f;
    [SerializeField] private float chargeDistance= 20.0f;
    [SerializeField] private float contactDmg = 10.0f;
    [SerializeField] private float chargeDmg = 20.0f;

    [Header("Warning Settings")]
    private bool warned;
    [Header("Collision")]
    [SerializeField] private CharacterController playerCC;
    [SerializeField] private FirstPersonController playerController;
    [SerializeField] private Collider enemyCollider;
    private bool isCharging;
    [SerializeField] private float knockbackMagnitude = 10.0f;


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
        if (playerCC == null || playerController == null)
        {
            GameObject playerObj = GameObject.Find("Player");
            if (playerObj != null)
            {
                playerCC = playerObj.GetComponent<CharacterController>();
                playerController = playerObj.GetComponent<FirstPersonController>();
                playerHP = playerObj.GetComponent<PlayerHealth>();
            }
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

    private void PerformWarning()
    {
        navAgent.velocity = Vector3.zero;
        navAgent.SetDestination(transform.position);
        if (!canAttack)
        {
            return;
        }

        if (playerTransform != null && !isOnAttackCooldown && !warned)
        {
            StartCoroutine(ChargeWarning());
        }
    }
    private IEnumerator ChargeWarning()
    {
        Vector3 target = playerTransform.position;
        target.y = transform.position.y;
        transform.LookAt(target);
        if (Physics.Raycast(warningOrigin.position, Vector3.down, out RaycastHit hit, 3.0f, terrainLayer))
        {
            Debug.Log("Aiming");
            Vector3 warningPosition = hit.point + Vector3.up * 0.05f;
            Instantiate(warningPrefab, warningPosition, transform.rotation, transform);
            //PreCharge Animation
            warned = true;
            yield return new WaitForSeconds(1.5f);
            yield return StartCoroutine(PerformCharge());
            yield return StartCoroutine(PerformCharge());
            yield return StartCoroutine(PerformCharge());
            StartCoroutine(AttackCooldownRoutine());
        }
        //Attack script
        else
        {
            Debug.Log("ray not hitting ground");
        }
    }
    private IEnumerator PerformCharge()
    {
        Debug.Log("Chargeing");
        isCharging = true;
        Physics.IgnoreCollision(enemyCollider, playerCC, true);
        Vector3 targetPosition = playerTransform.position;
        Vector3 chargeDirection = (targetPosition - transform.position).normalized;
        float travelled = 0f;
        while (travelled < chargeDistance)
        {
            float step = chargeSpeed * Time.deltaTime;
            transform.position += chargeDirection * step; 
            travelled += step;
            yield return null;
        }
        Physics.IgnoreCollision(enemyCollider, playerCC, false);
        isCharging = false;
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
            PerformWarning();
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
        if (isPlayerVisible && isPlayerInRange && !warned)
        {
            PerformWarning();
            return;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 direction = (other.transform.position - transform.position).normalized;
            playerController.AddKnockback(direction , knockbackMagnitude);
            if (isCharging)
            {
                //Stun effect
                playerHP.TakeDamage(chargeDmg);
                playerHP.CameraEffect();
                return;
            }
        }
    }
}
