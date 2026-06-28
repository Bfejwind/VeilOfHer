using UnityEngine;
using System.Collections;
using StarterAssets;

public class WeaponChargeddShot : MonoBehaviour
{
    public Camera playerCamera;
    //Shooting
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;
    //Charged
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;
    //Spread
    public float spreadIntensity;
    [Header("Normal Bullet Settings")]
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletLifetime = 3f;
    public float baseDamage = 10.0f;
    [Header("Chargedd Bullet Settings")]
    [SerializeField] private GameObject chargedBulletPrefab;
    [SerializeField] private float chargedSpeed;
    [SerializeField] private float chargedTime;
    [SerializeField] private float maxChargeTime;
    [SerializeField] private float maxChargeDamage;
    private bool previousFire;
    private bool isCharging;
    //Player input reference
    private StarterAssetsInputs _input;
    [Header("Player Stats")]
    private PlayerBehaviour playerStats;
    [Header("Loading")]
    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;
    //Audio
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] keyboardSound;
    [SerializeField] private AudioClip reloadSfx;
    
    public enum ShootingMode
    {
        Single,
        Charged,
        Auto
    }
    public ShootingMode currentShootingMode;
    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        playerStats = GetComponent<PlayerBehaviour>();
        _input = GetComponent<StarterAssetsInputs>();
        bulletsLeft = magazineSize;
    }

    // Update is called once per frame
    void Update()
    {
        ChargingWeapon();
        //Ammo UI
        if (AmmoManager.Instance.ammoDisplay != null)
        {
            AmmoManager.Instance.ammoDisplay.text = $"{bulletsLeft}/{magazineSize}";
        }
    }

    private void ChargingWeapon()
    {
        //Debug.Log($"fire: {_input.fire}, previousFire: {previousFire}");
        //Start Charging
        if (_input.fire && !previousFire)
        {
            Debug.Log("FirePressed");
            chargedTime = 0f;
        }
        if (_input.fire)
        {
            Debug.Log("Firecounting");
            chargedTime += Time.deltaTime;
            chargedTime = Mathf.Min(chargedTime, maxChargeTime);
        }
        if (!_input.fire && previousFire)
        {
            Debug.Log("FireReleased");
            FireChargedShot(chargedTime);
        }
        previousFire = _input.fire;
    }
    private void FireChargedShot(float charge)
    {
        readyToShoot = false;
        bulletsLeft--;
        float finalDamage = (baseDamage + playerStats.bulletDamageUpgrade)* (charge/maxChargeTime*maxChargeDamage) * playerStats.bulletDamageMultiplier;
        Debug.Log($"Bullet Damage: {finalDamage}");
        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;
        //Instantiate bullet
        GameObject bigBullet = Instantiate(chargedBulletPrefab, bulletSpawn.position, Quaternion.identity);
        //Calculate damage
        bigBullet.GetComponent<Bullet>().DamageCalculation(finalDamage);
        //Point at shooting direction
        bigBullet.transform.forward = shootingDirection;
        //Apply force to bullet
        bigBullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized*bulletVelocity, ForceMode.Impulse);
        //destroy bullet
        StartCoroutine(destroyBulletAfterTime(bigBullet,bulletLifetime));
        if (allowReset)
        {
            Invoke("ResetShot",shootingDelay);
            allowReset = false;
        }
        //Charged mode
        if (currentShootingMode == ShootingMode.Charged && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
        audioSource.PlayOneShot(keyboardSound[UnityEngine.Random.Range(0, keyboardSound.Length)]);
    }
    private void OnReload()
    {
        if (bulletsLeft < magazineSize && isReloading == false)
        {
            isReloading = true;
            audioSource.PlayOneShot(reloadSfx);
            Invoke("ReloadCompleted", reloadTime);
        }
    }
    private void ReloadCompleted()
    {
        bulletsLeft = magazineSize;
        isReloading = false;

    }

    public void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }
    public Vector3 CalculateDirectionAndSpread()
    {
        //Shoot from middle of screen to check where we are pointing
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray,out hit))
        {
            //Hits something, store what you hit
            targetPoint = hit.point;
        }
        else
        {
            //Shooting into air,get direction where bullet shd fly off
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;
        float x = UnityEngine.Random.Range(-spreadIntensity,spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity,spreadIntensity);

        //Return shooting direction and spread
        return direction + new Vector3(x,y,0);
    }
    private IEnumerator destroyBulletAfterTime (GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
