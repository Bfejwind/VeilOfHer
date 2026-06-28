using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Camera playerCamera;
    //Shooting
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;
    //Burst
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
    [Header("Charged Bullet Settings")]
    [SerializeField] private GameObject chargedBulletPrefab;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float chargeTime;
    private bool isCharging;
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
        Burst,
        Auto
    }
    public ShootingMode currentShootingMode;
    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        playerStats = GetComponent<PlayerBehaviour>();
        bulletsLeft = magazineSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentShootingMode == ShootingMode.Auto)
        {
            //Holding down left mouse
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
        {
            // if (Input.GetKey(KeyCode.Mouse0) && !isCharging)
            // {
            //     isCharging = Input.GetKey(KeyCode.Mouse0);
            // }
            //Clicking once left mouse
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }
        if (readyToShoot && isShooting && bulletsLeft > 0)
        {
            burstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }
        // if (readyToShoot && isCharging && bulletsLeft > 0)
        // {
        //     ChargedShot();
        // }
        //Ammo UI
        if (AmmoManager.Instance.ammoDisplay != null)
        {
            AmmoManager.Instance.ammoDisplay.text = $"{bulletsLeft}/{magazineSize}";
        }
    }

    private void FireWeapon()
    {
        readyToShoot = false;
        bulletsLeft--;
        float finalDamage = (baseDamage + playerStats.bulletDamageUpgrade) * playerStats.bulletDamageMultiplier;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;
        //Instantiate bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        //Calculate damage
        bullet.GetComponent<Bullet>().DamageCalculation(finalDamage);
        //Point at shooting direction
        bullet.transform.forward = shootingDirection;
        //Apply force to bullet
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized*bulletVelocity, ForceMode.Impulse);
        //destroy bullet
        StartCoroutine(destroyBulletAfterTime(bullet,bulletLifetime));
        //Check if done shooting
        if (allowReset)
        {
            Invoke("ResetShot",shootingDelay);
            allowReset = false;
        }
        //Burst mode
        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
        audioSource.PlayOneShot(keyboardSound[UnityEngine.Random.Range(0, keyboardSound.Length)]);
    }
    // private void ChargedShot()
    // {
    //     readyToShoot = false;
    //     bulletsLeft--;
    //     float finalDamage = (baseDamage + playerStats.bulletDamageUpgrade) * playerStats.bulletDamageMultiplier;

    //     Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;
    //     //Instantiate bullet
    //     GameObject bigBullet = Instantiate(chargedBulletPrefab, bulletSpawn.position, Quaternion.identity);
    //     //Calculate damage
    //     bigBullet.GetComponent<Bullet>().DamageCalculation(finalDamage);
    //     //Point at shooting direction
    //     bigBullet.transform.forward = shootingDirection;
    //     //Apply force to bullet
    //     bigBullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized*bulletVelocity, ForceMode.Impulse);
    //     //destroy bullet
    //     StartCoroutine(destroyBulletAfterTime(bigBullet,bulletLifetime));
    // }
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
