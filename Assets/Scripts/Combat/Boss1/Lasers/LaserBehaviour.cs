using System.Collections;
using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{
    private Rigidbody FloatRB;
    [SerializeField] private float rotationSpeed = 10.0f;
    [SerializeField] private float damage = 20.0f;
    [SerializeField] private GameObject laserHolder;
    [SerializeField] private float laserDownTime = 10.0f;
    public bool laserOn = true;
    //[Header("Layers")]
    void Start()
    {
        FloatRB = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.World);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth playerHP))
        {
            if (playerHP.IsInvulnerable)
            {
                return;
            }
            else
            {
                playerHP.TakeDamage(damage);
            }
        }
        if (other.CompareTag("DisableLaser"))
        {
            StartCoroutine(DeactivateLaser());
        }
    }
    private IEnumerator DeactivateLaser()
    {
        //Laser down SFX
        laserOn = false;
        laserHolder.SetActive(false);
        yield return new WaitForSeconds(laserDownTime);
        //Laser Up SFX
        laserHolder.SetActive(true);
        laserOn = true;
    }
}
