using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    isAlive,
    isDead,
}

public enum CauseOfDeath
{
    Spike,
    Enemy,
    Default,
}

public class PlayerMvmt : MonoBehaviour
{
    public static State playerState;
    public static CauseOfDeath deathCause;

    [SerializeField] private CharacterController playerController;
    [SerializeField] private Transform cam;
    
    [SerializeField] private Animator animator;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask ground;

    private float horizontalInput;
    private float verticalInput;
    private Vector3 faceDirection;

    [SerializeField] private float maximumSpeed = 10f;
    private float turnSmoothTime = 0.1f; // so player direction does not snap to place
    private float turnSmoothVelocity;
    private Vector3 moveDirection;
    private float originalStepOffset;

    //jumping
    private float gravityVal = 9.81f;
    private float vSpeed = 0; // current vertical velocity
    [SerializeField] private float jumpSpeed = 1.5f;

    float targetAngle;
    float angle;

    private float currentSpeed = 0f;



    private void Start()
    {
        playerState = State.isAlive;
        deathCause = CauseOfDeath.Default;
        originalStepOffset = playerController.stepOffset;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        switch (playerState)
        {
            case State.isAlive:
                Move();
                Jump();
                break;

            case State.isDead:
                animator.applyRootMotion = true;

                switch (deathCause)
                {
                    case CauseOfDeath.Spike:
                        animator.SetTrigger("DeathForward");
                        break;
                    case CauseOfDeath.Enemy:
                        animator.SetTrigger("DeathLaunch");
                        break;
                }
                break;
        }
        
    }


    private void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        faceDirection = new Vector3(horizontalInput, 0, verticalInput);

        //Vector2 vectorNoY = new Vector2(faceDirection.x, faceDirection.z);

        Vector3 projDirection = Vector3.ProjectOnPlane(faceDirection, Vector3.up);

        // WASD movement
        if (projDirection.magnitude >= 0.1f)
        { // not sure why this is necessary, maybe it accounts for controller drift?
            //currentSpeed = maximumSpeed;

            // rotate player y-direction to point where they are going
            // Atan2(a, b) -> returns angle (rad) btwn x-axis & vector starting at (0,0) terminating at (a, b)
            // in this case, 0 degrees is when player is forward (normally 90 degrees)
            targetAngle = Mathf.Atan2(faceDirection.x, faceDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            // smooth turning
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            if (TouchingGround())
            {
                animator.SetBool("MoveForward", true);
            }
            else
            {
                animator.SetBool("MoveForward", false);
            }
            
        }
        else
        {
            //currentSpeed = 0;
            animator.SetBool("MoveForward", false);
        }

        // point the right way, and move in the right way
        moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward * projDirection.magnitude;
        playerController.Move(moveDirection * (maximumSpeed * Time.deltaTime) + new Vector3(0, vSpeed, 0) * Time.deltaTime);
    }

    // sets vSpeed based on TouchingGround and if Space pressed
    private void Jump()
    {
        if (TouchingGround())
        {
            animator.SetBool("Grounded", true);
            // stop vertical speed dropping infinitely when grounded
            if (vSpeed < 0.0f)
            {
                vSpeed = -2f;
            }

            if (Input.GetButtonDown("Jump"))
            {
                vSpeed = Mathf.Sqrt(jumpSpeed * 2f * gravityVal);
                animator.SetTrigger("Jump");
            }
        }
        else
        {
            animator.SetBool("Grounded", false);
        }
        vSpeed -= gravityVal * Time.deltaTime;
    }

    private bool TouchingGround() 
    {
        return Physics.CheckSphere(groundCheck.position, .05f, ground);
    }

    public void AnimateDeathForward()
    {
        animator.SetTrigger("DeathForward");
    }
}
