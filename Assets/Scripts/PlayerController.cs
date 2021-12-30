using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //movement components
    private  CharacterController controller;
    private  Animator animator;  

    private float moveSpeed = 4f;

    [Header("Movement Speed")]
    public float walkSpeed = 4f;
    public float runSpeed = 8f;

    //Interaction Components
    PlayerInteractions playerInteraction;


    // Start is called before the first frame update
    void Start()
    {
        //Get movement components
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        //Get Interactions Components
        playerInteraction = GetComponentInChildren<PlayerInteractions>();
    }

    // Update is called once per frame
    void Update()
    {
        move();

        //runs the function that handles all interatcion
        Interact();

        //For Debugging Purposes Only, Should speed up time as long as its held down
        if(Input.GetKey(KeyCode.RightBracket)){
        TimeManager.Instance.Tick();
    }
    }

    public void Interact(){
        if(Input.GetButtonDown("Fire1")){
            playerInteraction.Interact();
        }
    }

    public void move(){
        //Grabbing vertical and horizontal inputs (WASD/arrows) as numerical values.
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //Direction normalized in a vector
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 velocity = moveSpeed * Time.deltaTime * direction;

        //is sprint key pressed
        if(Input.GetButton("Sprint")){
            //set the animation to run instead of walk and increase speed
            moveSpeed = runSpeed;
            animator.SetBool("Running", true);

        } else {
            //Set the animation to walk and decrease our speed
            moveSpeed = walkSpeed;
            animator.SetBool("Running", false);
        }

        //Check if there is movement
        if(direction.magnitude >= 0.1f)
        {
            //Look in that direction 
            transform.rotation = Quaternion .LookRotation(direction);

            //move
            controller.Move(velocity);
        }

        animator.SetFloat("Speed", velocity.magnitude);


    }
}
