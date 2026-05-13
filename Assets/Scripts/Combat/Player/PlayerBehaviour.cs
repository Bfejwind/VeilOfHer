using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("References")]
    public bool shieldReady;
    public bool shieldDeployed;
    [SerializeField] GameObject reflectProjectile;
    [SerializeField] GameObject shieldObject;

    void Awake()
    {
        shieldObject.SetActive(false);
        shieldReady = true;
        shieldDeployed = false;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (shieldReady && !shieldDeployed)
        {
            //Clicking once right mouse
            shieldDeployed = Input.GetKeyDown(KeyCode.Mouse1);
            Debug.Log("Right Clicked");
        }
        if (shieldReady && shieldDeployed)
        {
            StartCoroutine(Shield());
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            reflectProjectile = other.gameObject;
            Debug.Log("Acquired: "+ other.gameObject.name);
            if (shieldDeployed)
            {
                Rigidbody reflectProjectileRB = reflectProjectile.GetComponent<Rigidbody>();
            }
            
        }
    }
    IEnumerator Shield()
    {
        shieldObject.SetActive(true);
        shieldReady = false;
        shieldDeployed = true;
        yield return new WaitForSeconds(1.0f);
        shieldObject.SetActive(false);
        shieldReady = true;
        shieldDeployed = false;
    }
}
