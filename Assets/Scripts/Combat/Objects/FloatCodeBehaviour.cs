using System.Collections;
using UnityEngine;

public class FloatCodeBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private float floatStrength;
    private Rigidbody FloatRB;
    [Header("Movement Area Limits")]
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = 0f;
    public float maxY = 7f;
    public float minZ = -10f;
    public float maxZ = 10f;
    [Header("Behavior")]
    [Tooltip("If checked, object bounces off walls. If unchecked, it stops")]
    public bool bounceOffWalls = true;
    void Start()
    {
        FloatRB = GetComponent<Rigidbody>();
        StartCoroutine(FloatRandomly());
    }
    void Update()
    {
        // Vector3 pos = transform.position;
        // Vector3 vel = FloatRB.linearVelocity; 

        // // --- X Axis (Left / Right) ---
        // if (pos.x < minX) { pos.x = minX; if (bounceOffWalls) vel.x = -vel.x; else vel.x = 0; }
        // else if (pos.x > maxX) { pos.x = maxX; if (bounceOffWalls) vel.x = -vel.x; else vel.x = 0; }

        // // --- Y Axis (Up / Down) ---
        // if (pos.y < minY) { pos.y = minY; if (bounceOffWalls) vel.y = -vel.y; else vel.y = 0; }
        // else if (pos.y > maxY) { pos.y = maxY; if (bounceOffWalls) vel.y = -vel.y; else vel.y = 0; }

        // // --- Z Axis (Forward / Backward) ---
        // if (pos.z < minZ) { pos.z = minZ; if (bounceOffWalls) vel.z = -vel.z; else vel.z = 0; }
        // else if (pos.z > maxZ) { pos.z = maxZ; if (bounceOffWalls) vel.z = -vel.z; else vel.z = 0; }

        // // Apply corrected position and velocity back to physics engine
        // transform.position = pos;
        // FloatRB.linearVelocity = vel;
    }
    IEnumerator FloatRandomly()
    {
        while (true)
        {
            floatStrength = Random.Range(5,10);
            Vector3 randomDirection = Random.insideUnitSphere.normalized;
            FloatRB.AddForce(randomDirection * floatStrength,ForceMode.Impulse);
            yield return new WaitForSeconds(2.0f);
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 center = new Vector3((minX + maxX) / 2, (minY + maxY) / 2, (minZ + maxZ) / 2);
        Vector3 size = new Vector3(maxX - minX, maxY - minY, maxZ - minZ);
        Gizmos.DrawWireCube(center, size);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            //print("hit" + collision.gameObject.name);
            Destroy(gameObject);
        }
    }


}
