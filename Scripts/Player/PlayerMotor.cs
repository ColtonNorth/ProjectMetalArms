using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private bool lerpCrouch;
    private bool crouching;
    private bool sprinting;
    private bool audioStillPlaying = false;
    private float crouchTimer;
    public float gravity = -9.81f;
    public float speed = 5f;
    public float jumpHeight = 3f;
    public float numJumps = 2f;

    public AudioSource playerAudio;
    [SerializeField] AudioClip playerLandMetal;
    [SerializeField] AudioClip[] playerStepMetal;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        //playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
        if(isGrounded)
        {
            numJumps = 2f;
        }
        if(lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            if(crouching)
                controller.height = Mathf.Lerp(controller.height, 1, p);
            else
                controller.height = Mathf.Lerp(controller.height, 2, p);
            if(p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
    }

    //Crouching functionality
    public void Crouch()
    {
        crouching = !crouching;
        crouchTimer = 0;
        lerpCrouch = true;
    }
    //Sprinting functionality
    public void Sprint()
    {
        sprinting = !sprinting;
        if(sprinting)
            speed = 8;
        else
            speed = 5;
    }

    //This function receives inputs from InputManager.cs and applys the movement to our character controller.
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;
            if(input.x != 0 || input.y != 0)
            {
                //Play audio of player moving
                if(!audioStillPlaying)
                {
                    StartCoroutine(MovementAudio());
                }
            }
            
        controller.Move(playerVelocity * Time.deltaTime);
        //Debug.Log(playerVelocity.y);
    }

    //This function enables jumping for the character, given that they are on the ground currently.
    public void Jump()
    {
        if(numJumps > 1)
        {
            //grounded jump.
            if(isGrounded)
            {
                playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
                numJumps -= 1;
            }
            //Do an air jump (double jump)
            if(!isGrounded)
            {
                playerVelocity.y = Mathf.Sqrt(jumpHeight * -4.0f * gravity);
                numJumps -= 1;
            }
        }
        
    }

    IEnumerator MovementAudio()
    {
            if(!isGrounded)
            {
                yield break;
            }
            audioStillPlaying = true;
            AudioClip clip = playerStepMetal[UnityEngine.Random.Range(0, playerStepMetal.Length)];
            playerAudio.volume = 0.2f;
            playerAudio.PlayOneShot(clip);
            if(sprinting)
                yield return new WaitForSeconds(0.2f);
            else
                yield return new WaitForSeconds(0.3f);
            audioStillPlaying = false;
    }
}
