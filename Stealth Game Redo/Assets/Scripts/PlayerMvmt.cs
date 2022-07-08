
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMvmt : MonoBehaviour
{
    [SerializeField] private CharacterController playerController;
    [SerializeField] private Transform cam;
    
    [SerializeField] private Animator animator;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask ground;

    private float horizontalInput;
    private float verticalInput;
    private Vector3 faceDirection;

    private float speed = 5f;
    private float turnSmoothTime = 0.1f; // so player direction does not snap to place
    private float turnSmoothVelocity;
    private Vector3 moveDirection;
    private float originalStepOffset;

    //jumping
    private float gravityVal = 9.81f;
    private float vSpeed = 0; // current vertical velocity
    private float jumpSpeed = 2.5f;

    float targetAngle;
    float angle;



    private void Start()
    {
        originalStepOffset = playerController.stepOffset;
    }

    private void Update()
    {

        Move();
        Jump();

    }

    private void Move()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        faceDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

        Vector2 vectorNoY = new Vector2(faceDirection.x, faceDirection.z);

        //Vector3 projDirection = Vector3.ProjectOnPlane(direction, Vector3.up);
        Debug.Log("vectorNoY: " + vectorNoY.magnitude);
        // WASD movement
        if (vectorNoY.magnitude >= 0.1f)
        { // not sure why this is necessary, maybe it accounts for controller drift?
            speed = 3.5f;

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
            speed = 0;
            animator.SetBool("MoveForward", false);
        }

        // point the right way, and move in the right way
        moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
        playerController.Move(moveDirection.normalized * (speed * Time.deltaTime) + new Vector3(0, vSpeed, 0) * Time.deltaTime);
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

            if (Input.GetKeyDown(KeyCode.Space))
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
}
