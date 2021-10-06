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

    [SerializeField]
    int currentAmmo;

    //Weapon Effects
    //MuzzleFlash
    public ParticleSystem muzzleFlash;
    //Eject bullet casin
    public ParticleSystem bulletCasing;

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
    }


    void Shoot()
    {
        if(Time.time > nextFire)
        {
            nextFire = Time.time + rateOfFire;

            currentAmmo--;

            StartCoroutine(WeaponEffects());

            if(Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, weaponRange))
            {
                if(hit.transform.tag == "Enemy")
                {
                    EnemyHealth enemyHealthScript = hit.transform.GetComponent<EnemyHealth>();
                    enemyHealthScript.DeductHealth(damageEnemy);
                } 
                else
                {
                    Debug.Log("Hit Something Else");
                }
            }
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
