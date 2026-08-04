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
    private bool isHacked;
    private bool isHacking;
    private float currentHackAmt;
    private float maxHackAmt = 100.0f;
    public float hackIncrement = 10.0f;
    private float hackDecrement = 5.0f;
    [SerializeField] private GameObject hackBar;
    [SerializeField] private Slider hackSlider;
    private Coroutine hackCoroutine;

    void Awake()
    {
        shieldObject.SetActive(false);
        shieldReady = true;
        shieldDeployed = false;
        currentHackAmt = 0;
        hackSlider.value = currentHackAmt;
        hackSlider.maxValue = maxHackAmt;
        bossObject = GameObject.Find("Boss1");
        if (bossObject != null)
        {
            bossBehavior = bossObject.GetComponent<Boss1Behaviour>();
        }
        hackBar.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (bossBehavior != null)
        {
            if (bossBehavior.laserArenaState && !isHacking)
            {
                Debug.Log("Hacking");
                StartCoroutine(HackingStarted());
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
        hackBar.SetActive(true);
        isHacking = true;
        while (bossBehavior.laserArenaState)
        {
            if (isHacked)
            {
                Debug.Log("Hacked");
                hackBar.SetActive(false);
                isHacking = false;
                yield break;
            }
            if (!bossVisible())
            {
                yield return new WaitForSeconds(0.5f);
                currentHackAmt += hackIncrement;
                UpdateHackBar();
                if (currentHackAmt == maxHackAmt)
                {
                    isHacked = true;
                }
            }
            else
            {
                yield return new WaitForSeconds(2.0f);
                currentHackAmt -= hackDecrement;
                UpdateHackBar();
            }
        }
        hackBar.SetActive(false);
        isHacking = false;
    }
    private void UpdateHackBar()
    {
        currentHackAmt = Mathf.Clamp(currentHackAmt,0,maxHackAmt);
        hackSlider.value = currentHackAmt;
    }
}
