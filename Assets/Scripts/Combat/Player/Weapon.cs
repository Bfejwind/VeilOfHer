using System;
using System.Collections;
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
    //Bullet
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletLifetime = 3f;
    
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
            //Clicking once left mouse
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }
        if (readyToShoot && isShooting)
        {
            burstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;
        //Instantiate bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
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
