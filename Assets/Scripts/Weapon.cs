using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    //Scope
    public Image scopeImg;
    //Guns
    public GameObject mainGunObject, sniperGunObject;
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
    public float bulletPrefabLifeTime = 3f;
    public GameObject muzzleEffect;
    public Animator animator;

    //Loading
    public int totalBullet;
    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;

    public enum WeaponModel
    {
        mainGun,Sniper
    }
    public WeaponModel thisWeaponModel;
    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currenShootingMode;

    private void Awake()
    {
        scopeImg.enabled = false;
        mainGunObject.SetActive(true);
        sniperGunObject.SetActive(false);
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        bulletsLeft = magazineSize;
        totalBullet = magazineSize * 3;
    }
    // Update is called once per frame
    void Update()
    {
        //Setting up the Gun Switch key
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

            GunSwitch();
            
            
        }
        //Setting up the zoom in function
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (thisWeaponModel == Weapon.WeaponModel.Sniper)
            {
                Camera.main.fieldOfView = 23;
                scopeImg.enabled = true;
            }
        }
        //Setting up the zoom out function when we release the button
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            Camera.main.fieldOfView = 60;
            scopeImg.enabled = false;
        }
        if (bulletsLeft==0 && isShooting)
        {
            SoundManager.Instance.emptyMagazineSoundMainGun.Play();
        }
        
        if (currenShootingMode== ShootingMode.Auto)
        {
            //Holding Down Left Mouse Button
            isShooting=Input.GetKey(KeyCode.Mouse0);
        }
        //Clicking Left Mouse Button Once
        else if (currenShootingMode==ShootingMode.Single ||currenShootingMode==ShootingMode.Burst)
        {
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (Input.GetKeyDown(KeyCode.R)&& bulletsLeft<magazineSize&& isReloading==false)
        {
            Reload();
        }

        //If you want to automatically reload when magazine is empty
        if (readyToShoot && isShooting==false && isReloading==false && bulletsLeft<=0)
        {
            //Reload();
        }
        if (readyToShoot&& isShooting && bulletsLeft>0)
        {
            burstBulletsLeft = bulletsPerBurst;
           
            FireWeapon();

            
        }
        if (AmmoManager.Instance.ammoDisplay!=null)
        {
            AmmoManager.Instance.ammoDisplay.text = $"{bulletsLeft / bulletsPerBurst}/";
            AmmoManager.Instance.totalAmmoDisplay.text=$"{totalBullet/bulletsPerBurst}";
        }





    }

    private void GunSwitch()
    {

        if (mainGunObject.activeInHierarchy == true || sniperGunObject.activeInHierarchy == false)
        {
            animator.SetTrigger("GUNSWITCH");
            Invoke("EquipSniper", 1);
            print("aaaaa");
        }

        if (sniperGunObject.activeInHierarchy == true || mainGunObject.activeInHierarchy == false)
        {
            animator.SetTrigger("GUNSWITCH");
            Invoke("EquipMainGun", 1);
            print("aaaaa");

        }
    }

    void EquipMainGun()
    {
        sniperGunObject.SetActive(false);
        mainGunObject.SetActive(true);
        
    }
    void EquipSniper()
    {
        mainGunObject.SetActive(false);
        sniperGunObject.SetActive(true);
        
    }



    private void FireWeapon()
    {
        bulletsLeft--;

        muzzleEffect.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("RECOIL");

        //SoundManager.Instance.shootingSoundMainGun.Play();
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        readyToShoot = false;
        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;


        //Instantiate the bullet type
       

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        

        //Pointing the bullet to face the shooting direction
        bullet.transform.forward = shootingDirection;
        

        //Shoot the bullet
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);
        


        //Destroy the bullet after some time
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));
        

        //Checking if we are done shooting
        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }
        //Burst Mode
        if (currenShootingMode==ShootingMode.Burst&& burstBulletsLeft>1) // We already shoot once before this checks
        {
            burstBulletsLeft--;
            Invoke("FireWeapon",shootingDelay);
        }


    }

    private void Reload()
    {
        SoundManager.Instance.PlayReloadSound(thisWeaponModel);
        animator.SetTrigger("RELOAD");
        isReloading = true;
        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        totalBullet = totalBullet - (magazineSize - bulletsLeft);
        bulletsLeft = magazineSize;
        isReloading = false;
    }
    private void ResetShot()
    { readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        //Shooting from the middle of the screen to check where are we pointing 
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray,out hit))
        {
            //Hitting something
            targetPoint = hit.point;
        }
        else
        {
         //Shooting at the air
         targetPoint=ray.GetPoint(100);
        }
        Vector3 direction=targetPoint-bulletSpawn.position;

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        //Returning the shooting direction and spread
        return direction + new Vector3(x, y, 0);



    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
