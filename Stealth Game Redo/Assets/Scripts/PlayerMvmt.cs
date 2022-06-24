
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMvmt : MonoBehaviour
{
    public CharacterController playerController;
    public Transform cam;
    Vector3 moveDir;

    public float speed = 5f;
    private float originalStepOffset;
    public float turnSmoothTime = 0.1f; // so player direction does not snap to place
    private float turnSmoothVelocity;

    //float jumpHeight = 1.0f;
    public float gravityVal = 9.81f;
    float vSpeed = 0; // current vertical velocity
    public float jumpSpeed = 3.5f;
    public float jumpSpeedMoving = 3f;

    void Start()
    {
        originalStepOffset = playerController.stepOffset;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        // if (playerController.isGrounded && Input.GetKeyDown(KeyCode.Space)) {
        //     Debug.Log("jump girl jump");
        //     direction.y =  jumpSpeed - (gravityVal * Time.deltaTime);
        // }

        if (playerController.isGrounded) {
            playerController.stepOffset = originalStepOffset;
            vSpeed = 0;
            GetComponent<Animator>().SetBool("Grounded", true);
            //Debug.Log("on the ground");
            if (Input.GetKeyDown(KeyCode.Space)) {
                vSpeed = jumpSpeed;
                GetComponent<Animator>().SetTrigger("Jump");
                GetComponent<Animator>().SetBool("Grounded", false);
            }
        }
        else {
            playerController.stepOffset = 0; // fixes jumping against wall glitch
        }

        // apply gravity
        vSpeed -= gravityVal * Time.deltaTime;

        Vector3 jumpVector = new Vector3(0, vSpeed, 0);

        playerController.Move(jumpVector * speed * Time.deltaTime); // jump even if not WASD moving

        Vector2 vectorNoY = new Vector2(direction.x, direction.z);
        

        // WASD movement
        if (vectorNoY.magnitude >= 0.1f) { // not sure why this is necessary, maybe it accounts for controller drift?


            // rotate player y-direction to point where they are going
            // Atan2(a, b) -> returns angle (rad) btwn x-axis & vector starting at (0,0) terminating at (a, b)
            // in this case, 0 degrees is when player is forward (normally 90 degrees)
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            // smooth turning
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            // point the right way, and move in the right way
            moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            playerController.Move(moveDir.normalized * speed * Time.deltaTime);
            GetComponent<Animator>().SetBool("MoveForward", true);
        }
        else {
            GetComponent<Animator>().SetBool("MoveForward", false);
        }


        if (playerController.isGrounded && Input.GetKeyDown(KeyCode.Space)) {
            direction.y = Mathf.Sqrt(jumpSpeed * 2f * gravityVal);
            GetComponent<Animator>().SetTrigger("Jump");
        }

        direction.y -= gravityVal * Time.deltaTime;
        playerController.Move(direction * Time.deltaTime);

        
    }
}
