using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerXp : MonoBehaviour
{
    private float Xp;
    private float startingXp = 0f;
    private float maxXp = 100f;
    private float playerLevel = 0f;
    public Image frontXpBar;

    public AudioSource xpAudio;
    [SerializeField] AudioClip levelUp;

    // Start is called before the first frame update
    void Start()
    {
        Xp = startingXp;
    }

    // Update is called once per frame
    void Update()
    {
        Xp = Mathf.Clamp(Xp, 0, maxXp);
        UpdateXpUI();
        //Placeholder for gaining XP
        if(Input.GetKeyDown(KeyCode.M))
        {
            GainXp(Random.Range(5, 10));
        }
        //Player has leveled up.
        if(Xp == 100)
        {
            xpAudio.PlayOneShot(levelUp);
            playerLevel += 1;
            Xp = 0;
        }
    }

    public void UpdateXpUI()
    {
        float xFraction = Xp / maxXp;
        //We are only increasing the frontXpBar, so no need to worry about the background one.
        frontXpBar.fillAmount = xFraction;
    }

    public void GainXp(float amount)
    {
        Xp += amount;
    }
}
