using UnityEngine;
using System.Collections;

public class HeavyLazerGun : MonoBehaviour
{
    public float damage = 30f;
    public float range = 200f;
    public float impactForce = 80f;
    public float fireRate = 2f;
    private float nextTimeToFire = 0f;
    public float clipSize = 15f;
    public float ammo;
    public float reloadDuration = 2f;
    private bool isReloading = false;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    public AudioSource gunAudio;
    [SerializeField] AudioClip reloadGunAudio;

    void Start()
    {
        ammo = clipSize;
    }

    void OnEnable()
    {
        isReloading = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isReloading)
            return;

        if(Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        //Reload gun when R is pressed, or when out of ammo
        if(Input.GetKeyDown(KeyCode.R) && ammo != clipSize)
        {
            StartCoroutine(Reload());
        }
        if(ammo <= 0)
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();
        gunAudio.Play();
        ammo -= 1f;

        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            Grunt target = hit.transform.GetComponent<Grunt>();
            if(target != null)
            {
                target.TakeDamage(damage);
            }

            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 3f);
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading Heavy Lazer Gun...");
        gunAudio.PlayOneShot(reloadGunAudio);
        yield return new WaitForSeconds(reloadDuration);
        ammo = clipSize;
        isReloading = false;
    }
}
