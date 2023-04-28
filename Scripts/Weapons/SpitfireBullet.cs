using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitfireBullet : MonoBehaviour
{
    public float bulletDamage = 8f;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Enemy")
        {
            Debug.Log("Bullet hit grunt");
            collision.gameObject.GetComponent<Grunt>().TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
        else if(collision.transform.tag == "Player")
        {
            Debug.Log("Bullet collided with player");
        }
        else
            Debug.Log("Bullet hit something");
            Destroy(gameObject);
        
    }
}
