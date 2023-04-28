using UnityEngine;
using System.Collections;

public class AssaultRifle : MonoBehaviour
{
    //public float damage = 8f;
    public float range = 100f;
    public float impactForce = 30f;
    public float fireRate = 12f;
    public float clipSize = 50f;
    public float ammo, spread;
    public float reloadDuration = 2f;
    private float nextTimeToFire = 0f;
    private bool isReloading = false;

    //bullet 
    public GameObject bullet;

    //bullet force
    public float shootForce, upwardForce;

    public Camera fpsCam;
    public Transform attackPoint;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public Animator animator;

    public AudioSource gunAudio;
    [SerializeField] AudioClip ejectClip;
    [SerializeField] AudioClip insertClip;
    [SerializeField] AudioClip slapClip;
    
    void Start()
    {
        ammo = clipSize;
    }

    void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }
    
    // Update is called once per frame
    void Update()
    {
        if(isReloading)
            return;

        //Left mouse button pressed, fire gun.
        if(Input.GetButton("Fire1") && Time.time >= nextTimeToFire && ammo > 0)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
            
        }

        //Reload gun.
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

        //Find the exact hit position using a raycast
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //Just a ray through the middle of your current view
        RaycastHit hit;

        //check if ray hits something
        Vector3 targetPoint = transform.position;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75); //Just a point far away from the player

        //Calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        //Calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate new direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0); //Just add spread to last direction

        //Instantiate bullet/projectile
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity); //store instantiated bullet in currentBullet
        //Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;
        currentBullet.transform.Rotate(90f, 0f, 0f);

        //Add forces to bullet AKA shoot it.
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);
        if(currentBullet != null)
        {
            Destroy(currentBullet, 3f);
        }
        
        //Playing the impact effect for where the bullet lands.
        GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impactGO, 3f);
        
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        animator.SetBool("Reloading", true);
        gunAudio.PlayOneShot(ejectClip);
        yield return new WaitForSeconds(ejectClip.length);
        gunAudio.PlayOneShot(insertClip);
        yield return new WaitForSeconds(insertClip.length);
        gunAudio.PlayOneShot(slapClip);
        yield return new WaitForSeconds(slapClip.length);
        ammo = clipSize;
        isReloading = false;
        animator.SetBool("Reloading", false);
    }
}


