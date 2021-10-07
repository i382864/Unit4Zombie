using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMG : MonoBehaviour
{
    RaycastHit hit;

    //Used to damage enemy
    [SerializeField]
    float damageEnemy = 10f;

    [SerializeField]
    Transform shootPoint;

    
    public int currentAmmo = 30;
    public int maxAmmo = 30;
    public int carriedAmmo = 120;
    bool isReloading;

    //Weapon Effects
    //MuzzleFlash
    public ParticleSystem muzzleFlash;
    //Eject bullet casin
    public ParticleSystem bulletCasing;
    //Blood effect
    public GameObject bloodEffect;

    //RateOfFire
    [SerializeField]
    float rateOfFire;
    float nextFire = 0;

    [SerializeField]
    float weaponRange;

    void Start()
    {
        muzzleFlash.Stop();
        bulletCasing.Stop();
    }

    void Update()
    {
        if(Input.GetButton("Fire1")&& currentAmmo > 0)
        {
            Shoot();
        }
        if(Input.GetButton("Fire1")&& currentAmmo <= 0)
        {
           DryFire();
        }

        else if(Input.GetKeyDown(KeyCode.R) && currentAmmo <= 0)
        {
            Reload();
        }
    }


    void Shoot()
    {
        if(Time.time > nextFire)
        {
            nextFire = 0f;
            nextFire = Time.time + rateOfFire;
     

            currentAmmo--;

            StartCoroutine(WeaponEffects());

            ShootRay();

            
        }
    }

    void ShootRay()
    {
            if(Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, weaponRange))
            {
                if(hit.transform.tag == "Enemy")
                {
                    EnemyHealth enemyHealthScript = hit.transform.GetComponent<EnemyHealth>();
                    enemyHealthScript.DeductHealth(damageEnemy);
                    Instantiate(bloodEffect, hit.point, Quaternion.identity);
                } 
                else
                {
                    Debug.Log("Hit Something Else");
                }
            }
    }


    void DryFire()
    {
        if(Time.time > nextFire)
        {
            nextFire = 0f;
            nextFire = Time.time + rateOfFire;
     
        //add dryfire anim

        Debug.Log("Play Dry Fire Sound");

            
        }
    }

    void Reload()
    {
        if(carriedAmmo <= 0) return;
        StartCoroutine(ReloadCountdown(2f));
    }

    IEnumerator ReloadCountdown(float timer)
        {
            while(timer > 0f)
            {
                isReloading = true;
                timer -= Time.deltaTime;
                yield return null;
            }
            if(timer <= 0f)
            {
                isReloading = false;
                int bulletsNeededToFillMag = maxAmmo - currentAmmo;
                int bulletsToDeduct = (carriedAmmo >= bulletsNeededToFillMag) ? bulletsNeededToFillMag : carriedAmmo;

                carriedAmmo -= bulletsToDeduct;
                currentAmmo += bulletsToDeduct;
            }
        }
    

    IEnumerator WeaponEffects()
    {
        bulletCasing.Play();
        muzzleFlash.Play();
        yield return new WaitForEndOfFrame();
        muzzleFlash.Stop();
        bulletCasing.Stop();
    }
}
