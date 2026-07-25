using UnityEngine;
using System.Collections;
public class RockBehaviour : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10.0f;
    [SerializeField] private GameObject laserHolder;
    [SerializeField] private float laserDownTime = 10.0f;
    public bool laserOn = true;
    private void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.World);
    }
    void OnTriggerEnter(Collider other)
    {
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
