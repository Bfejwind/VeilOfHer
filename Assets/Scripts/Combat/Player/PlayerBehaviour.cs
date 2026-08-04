using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("References")]
    public bool shieldReady;
    public bool shieldDeployed;
    [SerializeField] GameObject reflectProjectile;
    [SerializeField] GameObject shieldObject;
    [Header("Stat modifiers")]
    public float bulletDamageMultiplier = 1f;
    public float abilityDamageMultiplier = 1f;
    public float bulletDamageUpgrade;
    [Header("LOOKATBOSS")]
    [SerializeField] private GameObject bossObject;
    private Boss1Behaviour bossBehavior;
    [SerializeField] private Camera playerCamera;
    private bool isHacking;
    private float currentHackAmt;
    private float maxHackAmt = 100.0f;
    public float hackIncrement = 10.0f;
    private float hackDecrement = 5.0f;
    [SerializeField] private GameObject hackBar;
    [SerializeField] private Slider hackSlider;

    void Awake()
    {
        shieldObject.SetActive(false);
        shieldReady = true;
        shieldDeployed = false;
        if (bossObject != null)
        {
            bossBehavior = bossObject.GetComponent<Boss1Behaviour>();
        }
        currentHackAmt = 0;
        hackSlider.value = currentHackAmt;
    }


    // Update is called once per frame
    void Update()
    {
        if (bossObject != null)
        {
            if (bossBehavior.laserRoutine1Started && !bossBehavior.laserRoutine1Ended)
            {
                if (!isHacking)
                {
                    hackBar.SetActive(true);
                    isHacking = true;
                }
                //Turn on hack Bar
                if (!bossVisible())
                {
                    StartCoroutine(HackingStarted());
                    //Increase Hack bar
                }
                else
                {
                    //reduce hack bar
                }
                
            }
        }
        // if (shieldReady && !shieldDeployed)
        // {
        //     //Clicking once right mouse
        //     shieldDeployed = Input.GetKeyDown(KeyCode.Mouse1);
        // }
        // if (shieldReady && shieldDeployed)
        // {
        //     Debug.Log("Right Clicked");
        //     StartCoroutine(Shield());
        // }
    }
    // void OnTriggerStay(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Bullet"))
    //     {
    //         reflectProjectile = other.gameObject;
    //         //Debug.Log("Acquired: "+ other.gameObject.name);
    //         if (shieldDeployed)
    //         {
    //             Rigidbody reflectProjectileRB = reflectProjectile.GetComponent<Rigidbody>();
    //         }
            
    //     }
    // }
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
    private bool bossVisible()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCamera);
        foreach (var plane in planes)
        {
            if (plane.GetDistanceToPoint(bossObject.transform.position) < 0)
            {
                return false;
            }
        }
        return true;
    }
    private IEnumerator HackingStarted()
    {
        while (!bossVisible())
        {
            yield return new WaitForSeconds(2.0f);
            currentHackAmt += hackIncrement;
            UpdateHackBar();
        }

    }
    private void UpdateHackBar()
    {
        hackSlider.value = currentHackAmt;
    }
}
