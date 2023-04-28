using UnityEngine;

public class Grunt : MonoBehaviour
{
    public float health = 50f;
    public float maxHealth = 50f;
    public float xpValue = 25f;
    private bool takenFirstHit = false;
    private bool belowHalfHealth = true;
    private bool gruntDead = false;

    [SerializeField] AudioClip[] hurtSounds;
    [SerializeField] AudioClip[] deathSounds;
    [SerializeField] AudioClip[] deathExplosions;
    AudioSource gruntAudio;
    


    void Start()
    {
        gruntAudio = GetComponent<AudioSource>();
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        {
            if(health <= 0f)
            {
                if(gruntDead == false)
                {
                    DeathSound();
                    gruntDead = true;
                    //Give the player XP for killing the Grunt;
                    GameObject.Find("Player").GetComponent<PlayerXp>().GainXp(xpValue);
                }
                
                //Commenting out die as it prevents death sound.
                //Die();
            }
            else
            {
                if(takenFirstHit == false)
                {
                    HurtSound();
                    takenFirstHit = true;
                }
                else if(health <= maxHealth/2f)
                {
                    if(belowHalfHealth == true)
                        HurtSound();
                        belowHalfHealth = false;
                }
                
            }
        }
    }

    //Event that takes place when target loses all health points
    void Die()
    {
        Destroy(gameObject);
    }

    //Play a random hurt sound from 1/5 options
    void HurtSound()
    {
        AudioClip clip = hurtSounds[UnityEngine.Random.Range(0, hurtSounds.Length)];
        gruntAudio.PlayOneShot(clip);
    }

    //Play a random death sound from 1/5 options
    void DeathSound()
    {
        AudioClip clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
        AudioClip explosion = deathExplosions[UnityEngine.Random.Range(0, deathExplosions.Length)];
        gruntAudio.PlayOneShot(clip);
        gruntAudio.PlayOneShot(explosion);
    }
}
