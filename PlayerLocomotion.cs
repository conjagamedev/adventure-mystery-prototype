using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    CharacterController playerController;
    Animator anim;  

    public float baseSpeed = 2.5f; 
    public float speed = 6f; 
    public float sprintSpeed = 4f; 
    public float gravity = -9.81f; 
    public float jumpStrength = 3f; 
    public float rollStrength = 2.8f; 
    
    public Transform cam;
    
    Vector3 velocity; 
    Vector3 moveDir; 

    float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f; 

    public bool isActive = false; 
    public bool canJump;
    public bool isFalling; 
    bool isGrounded; 
    public bool inRoll; 
    bool isSprint; 
    bool isClimbing; 
    public bool isHolding; 

    private float lastY; 
    public float FallingThreshold = 0.1f; 

    public float evadeTime = 1f; // this tells us how long the evade takes
    public float evadeDistance = 20f; // this tells us how far player will evade

    Vector3 colVec; 

    void Start()
    {
        playerController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        lastY = transform.position.y; 
    }
    void Update()
    {
        if(isActive)  
        {   
            if(!isClimbing && !isHolding)
            {
                HandleMovement();
                HandleJump();
                HandleGravity();
                HandleFall();
                HandleRoll();
                HandleSprint();  
            }   
            if(isClimbing)
            {
                HandleClimb();
            } 
            if(isHolding)
            {
                print("is holding & the value of is holding is " + isHolding);
                HandleHolding();
                HandleMovement();
                HandleGravity();
            }
        }
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized; 

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y; 
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; 
            playerController.Move(moveDir.normalized * speed * Time.deltaTime); 

            if(anim.GetBool("isJump") == false)
            {
                anim.SetBool("isMove", true); //play animation to run 
            }     
        }
        else
        {
            anim.SetBool("isMove", false);
            anim.SetBool("isSprint", false);
        }
    }

    void HandleGravity()
    {
        velocity.y += gravity * Time.deltaTime; 
        playerController.Move(velocity * Time.deltaTime);
    }

    void HandleJump()
    {
        if(velocity.y < 0 && !anim.GetBool("isFall"))
        {
            velocity.y = -3.5f; //changing gravity ! 
        }
        if(velocity.y < 0 && anim.GetBool("isFall") && !anim.GetBool("isJump"))
        {
            velocity.y = -12f; 
        }


        if(Input.GetButtonDown("Jump") && canJump && !inRoll && !isFalling)
        {
            velocity.y = Mathf.Sqrt(jumpStrength * -2 * gravity); // actual jumping 
            anim.SetBool("isJump", true);  
            StartCoroutine(EndJump());  
            canJump = false;
        }

        RaycastHit hit; 
        float dist = 0.1f; 
        Vector3 dir = new Vector3(0,-1,0);

        if(Physics.Raycast(transform.position, dir, out hit , dist))
        {
            canJump = true;
            isGrounded = canJump;
        }
        else
        {
            canJump = false;
            isGrounded = canJump;
        }

        IEnumerator EndJump()
        {
            yield return new WaitForSeconds(0.65F);
            anim.SetBool("isJump", false);
        }
    }

    void HandleFall()
    {
        float distancePerSecondSinceLastFrame = (transform.position.y - lastY) * Time.deltaTime; 
        lastY = transform.position.y; //set for next frame

        if(distancePerSecondSinceLastFrame < FallingThreshold && !isGrounded)
        {
           anim.SetBool("isFall", true); 
        }
        else
        {
            anim.SetBool("isFall", false);
        }
    }

    void HandleRoll()
    {
        if(Input.GetKeyDown(KeyCode.Q) && !inRoll && !isFalling && isGrounded)
        {   
            inRoll = true; 
            anim.SetBool("isRoll", true);
            StartCoroutine(waitRoll());    
        }

         IEnumerator waitRoll()
        {
            playerController.height = 0.65f; 
            playerController.center = new Vector3(0,0.3f, -0.08f);
            yield return new WaitForSeconds(0.4f);
            inRoll = false;
            playerController.height = 1.2f;
            playerController.center = new Vector3(0, 0.65f, -0.08f);
            anim.SetBool("isRoll", false);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Roll") && isSprint)
        {
            playerController.Move(transform.forward * (rollStrength + 0.8f) * Time.deltaTime);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Roll") && !isSprint)
        {
            playerController.Move(transform.forward * (rollStrength + 0.3f) * Time.deltaTime);
        }

    }

    void HandleSprint()
    {
        if(Input.GetKey(KeyCode.LeftShift) && !anim.GetBool("isSprint") && anim.GetBool("isMove") && !anim.GetBool("isFall") && !anim.GetBool("isJump"))
        {
            isSprint = true;
            speed = sprintSpeed; 
            anim.SetBool("inSprint", true);
                 
        }
        if(Input.GetKey(KeyCode.LeftShift) == false || !anim.GetBool("isMove"))
        {
            isSprint = false;
            speed = baseSpeed;
            anim.SetBool("inSprint", false);
        }
    }

    void HandleClimb()
    {
        if(isClimbing && anim.GetCurrentAnimatorStateInfo(0).IsName("ClimbUp"))
        {
            transform.Translate(Vector3.up * 1f * Time.deltaTime);  
        }

        if(isClimbing && anim.GetCurrentAnimatorStateInfo(0).IsName("FinalClimb"))
        {
            transform.Translate(Vector3.up * 0.8f * Time.deltaTime);  
        }
    }
    //triggers related for climbing!!
    void OnTriggerEnter(Collider col)
    {
        
        if(col.gameObject.CompareTag("Climb") && !anim.GetBool("isFall"))
        {
            anim.SetBool("isRoll", false);
            anim.SetBool("isFall", false);
            anim.SetBool("isJump", false);
            anim.SetBool("isMove", false);
            anim.SetBool("inClimbUp", true);
            isClimbing = true;  
        } 

        if(col.gameObject.CompareTag("ClimbTop") && !anim.GetBool("isFall"))
        {
            anim.SetBool("isRoll", false);
            anim.SetBool("isFall", false);
            anim.SetBool("isJump", false);
            anim.SetBool("isMove", false);
            anim.SetBool("inClimbUp", false);
            anim.SetBool("inClimbFinal", true);
            isClimbing = true;  
        } 
    }

    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.CompareTag("Climb"))
        {
            anim.SetBool("inClimbUp", false);
            playerController.Move(moveDir.normalized * (speed + 10) * Time.deltaTime);
        }

        if(col.gameObject.CompareTag("ClimbTop"))
        {
            StartCoroutine(WaitToStopClimb());
            anim.SetBool("inClimbUp", false);
            anim.SetBool("inClimbFinal", false);
            playerController.Move(moveDir.normalized * (speed + 10) * Time.deltaTime);
        }

    }

    void HandleHolding()
    {
        if(isHolding)
        {
            if(anim.GetBool("isMove") == true)
            {
                anim.SetBool("inSprint", false);
                anim.SetBool("inHold", true);
                anim.SetBool("isHolding&Idle", false);
            }
            else if(anim.GetBool("isMove") == false)
            {
                anim.SetBool("inSprint", false);
                anim.SetBool("isHolding&Idle", true);
                anim.SetBool("inHold", false);
            }   
        }
        
    }

    IEnumerator WaitToStopClimb()
    {
        yield return new WaitForSeconds(0.1f);
        isClimbing = false; 

    }
}
