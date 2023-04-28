using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : Interactable
{
    [SerializeField]
    private GameObject door;
    private bool doorOpen;
    public AudioSource keypadAudio;
    public AudioClip closeSound;
    public AudioClip openSound;

    // Start is called before the first frame update
    void Start()
    {
        keypadAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Interact()
    {
        doorOpen = !doorOpen;
        door.GetComponent<Animator>().SetBool("IsOpen", doorOpen);
        Sounds();
    }

    //Play door open/close sounds
    void Sounds()
    {
        if(!doorOpen)
        {
            keypadAudio.PlayOneShot(openSound);
        }
        else
        {
            keypadAudio.PlayOneShot(closeSound);
        }
    }
}
