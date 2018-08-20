using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Text so i can commit again

public class PlayerController : MonoBehaviour
{

    public float speed;
    private GameController controller;


    #region Variables

    //Components.
    Rigidbody rb;
    protected Animator animator;
    public GameObject target;
    [HideInInspector]
    public Vector3 targetDashDirection;
    public Camera sceneCamera;
    private UnityEngine.AI.NavMeshAgent agent;

    //Movement.
    [HideInInspector]
    public bool canMove = true;
    public float walkSpeed = 1.35f;
    float moveSpeed;
    public float runSpeed = 6f;
    float rotationSpeed = 40f;
    Vector3 inputVec;
    Vector3 newVelocity;
    public float slopeAmount = 0.5f;
    public bool onAllowableSlope;

    //Navmesh.
    public bool useNavMesh = false;
    private float navMeshSpeed;
    public Transform goal;

    //Jumping.
    public float gravity = -9.8f;
    [HideInInspector]
    public bool canJump;
    bool isJumping = false;
    [HideInInspector]
    public bool isGrounded;
    public float jumpSpeed = 12;
    public float doublejumpSpeed = 12;
    bool doublejumping = true;
    [HideInInspector]
    public bool canDoubleJump = false;
    [HideInInspector]
    public bool isDoubleJumping = false;
    bool doublejumped = false;
    bool isFalling;
    bool startFall;
    float fallingVelocity = -1f;
    float fallTimer = 0f;
    public float fallDelay = 0.2f;
    float distanceToGround;

    //Movement in the air.
    public float inAirSpeed = 8f;
    float maxVelocity = 2f;
    float minVelocity = -2f;

// Actions.
    [HideInInspector]
    public bool canAction = false;
    
    //Ta bort denna och allt kukar ur---
    [HideInInspector]
    public bool isStrafing = false;
    //----------------------------------

    [HideInInspector]
    public bool isDead = false;
    public float knockbackMultiplier = 1f;
    bool isKnockback;

    //Input variables.
    float inputHorizontal = 0f;
    float inputVertical = 0f;
    bool inputLightHit;
    bool inputDeath;
    bool inputJump;

    #endregion





    void Start()
    {
        GameObject tmp = GameObject.FindGameObjectWithTag("GameController");
        controller = tmp.GetComponent<GameController>();
        if (controller == null)
        {
            Debug.LogError("Unable to find the GameController script");
        }




        //Find the Animator component.
        if (animator = GetComponentInChildren<Animator>())
        {
        }
        else
        {
            Debug.LogError("ERROR: There is no animator for character.");
            Destroy(this);
        }
        //Use MainCamera if no camera is selected.
        if (sceneCamera == null)
        {
            sceneCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            if (sceneCamera == null)
            {
                Debug.LogError("ERROR: There is no camera in scene.");
                Destroy(this);
            }
        }
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.enabled = false;


    }

    void Inputs()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
        inputDeath = Input.GetButtonDown("Death");
        //inputJump = Input.GetButtonDown("Jump");
    }


    #region Updates
    void Update()
    {

        Inputs();
        if (canMove && !isDead && !useNavMesh)
        {
            CameraRelativeMovement();
        }
       
        Jumping();
        //if (inputLightHit && canAction && isGrounded)
        //{
        //    GetHit();
        //}
        if (inputDeath && canAction && isGrounded)
        {
            if (!isDead)
            {
                Death();
            }
            else
            {
                Revive();
            }
        }


        if (useNavMesh)
        {
            agent.enabled = true;
            navMeshSpeed = agent.velocity.magnitude;
        }
        else
        {
            agent.enabled = false;
        }

        //Slow time.
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (Time.timeScale != 1)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0.15f;
            }
        }
        //Pause.
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale != 1)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0f;
            }
        }

    }

    void FixedUpdate()
    {
        //float moveH = Input.GetAxis("Horizontal");
        //float moveV = Input.GetAxis("Vertical");

        //Vector3 movement = new Vector3(moveH, 0.0f, moveV);

        //Rigidbody body = GetComponent<Rigidbody>();
        //body.AddForce(movement * speed * Time.deltaTime);


        CheckForGrounded();
        //Apply gravity.
        rb.AddForce(0, gravity, 0, ForceMode.Acceleration);
        AirControl();
        //Check if character can move.
        if (canMove && !isDead)
        {
            moveSpeed = UpdateMovement();
        }
        //Check if falling.
        if (rb.velocity.y < fallingVelocity && useNavMesh != true)
        {
            isFalling = true;
            animator.SetInteger("Jumping", 2);
            canJump = false;
        }
        else
        {
            isFalling = false;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "pickup")
        {
            other.gameObject.SetActive(false);
            controller.AddScore(1);
        }

        else
        {
            //Debug.LogError("Unknown itemcollision");
        }



    }


    void LateUpdate()
    {
        if (!useNavMesh)
        {
            //Get local velocity of charcter
            float velocityXel = transform.InverseTransformDirection(rb.velocity).x;
            float velocityZel = transform.InverseTransformDirection(rb.velocity).z;
            //Update animator with movement values
            animator.SetFloat("Velocity X", velocityXel / runSpeed);
            animator.SetFloat("Velocity Z", velocityZel / runSpeed);
            //if character is alive and can move, set our animator
            if (!isDead && canMove)
            {
                if (moveSpeed > 0)
                {
                    animator.SetBool("Moving", true);
                }
                else
                {
                    animator.SetBool("Moving", false);
                }
            }
        }
        else
        {
            animator.SetFloat("Velocity X", agent.velocity.sqrMagnitude);
            animator.SetFloat("Velocity Z", agent.velocity.sqrMagnitude);
            if (navMeshSpeed > 0)
            {
                animator.SetBool("Moving", true);
            }
            else
            {
                animator.SetBool("Moving", false);
            }
        }
    }
    #endregion

    #region UpdateMovement

    /// <summary>
    /// Movement based off camera facing.
    /// </summary>
    void CameraRelativeMovement()
    {
        //converts control input vectors into camera facing vectors.
        Transform cameraTransform = sceneCamera.transform;
        //Forward vector relative to the camera along the x-z plane.
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;
        //Right vector relative to the camera always orthogonal to the forward vector.
        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        
        
        inputVec = inputHorizontal * right + inputVertical * forward;
    }

    /// <summary>
    /// Rotate character towards movement direction.
    /// </summary>
    void RotateTowardsMovementDir()
    {
        if (inputVec != Vector3.zero && !isStrafing)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(inputVec), Time.deltaTime * rotationSpeed);
        }
    }

    /// <summary>
    /// Applies velocity to rigidbody to move the character, and controls rotation if not targetting.
    /// </summary>
    /// <returns>The movement.</returns>
    float UpdateMovement()
    {
        if (!useNavMesh)
        {
            CameraRelativeMovement();
        }
        Vector3 motion = inputVec;
        if (isGrounded)
        {
            //Reduce input for diagonal movement.
            if (motion.magnitude > 1)
            {
                motion.Normalize();
            }
            if (canMove)
            {
                //Set speed by walking / running.
                if (isStrafing)
                {
                    newVelocity = motion * walkSpeed;
                }
                else
                {
                    newVelocity = motion * runSpeed;
                }
                //Rolling uses rolling speed and direction.

            }
        }
        else
        {
            //If falling, use momentum.
            newVelocity = rb.velocity;
        }
        if (!isStrafing || !canMove)
        {
            RotateTowardsMovementDir();
        }
        if (isStrafing)
        {
            //Make character face target.
            Quaternion targetRotation;
            Vector3 targetPos = target.transform.position;
            targetRotation = Quaternion.LookRotation(targetPos - new Vector3(transform.position.x, 0, transform.position.z));
            transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, (rotationSpeed * Time.deltaTime) * rotationSpeed);
        }
        newVelocity.y = rb.velocity.y;
        rb.velocity = newVelocity;
        //Return movement value for Animator component.
        return inputVec.magnitude;
    }

    #endregion

    #region Jumping

    /// <summary>
    /// Checks if character is within a certain distance from the ground, and markes it IsGrounded.
    /// </summary>
    void CheckForGrounded()
    {
        RaycastHit hit;
        Vector3 offset = new Vector3(0, 0.1f, 0);
        if (Physics.Raycast((transform.position + offset), -Vector3.up, out hit, 100f))
        {
            distanceToGround = hit.distance;
            if (distanceToGround < slopeAmount)
            {
                isGrounded = true;
                if (!isJumping)
                {
                    canJump = true;
                }
                startFall = false;
                doublejumped = false;
                canDoubleJump = false;
                isFalling = false;
                fallTimer = 0;
                if (!isJumping)
                {
                    animator.SetInteger("Jumping", 0);
                }
            }
            else
            {
                fallTimer += 0.009f;
                if (fallTimer >= fallDelay)
                {
                    isGrounded = false;
                }
            }
        }
    }

    void Jumping()
    {
        if (isGrounded)
        {
            if (canJump && inputJump)
            {
                
                //Method for jumping
                //StartCoroutine(_Jump());
                
            }
        }
        else
        {
            canDoubleJump = true;
            canJump = false;
            if (isFalling)
            {
                //Set the animation back to falling.
                animator.SetInteger("Jumping", 2);
                //Prevent from going into land animation while in air.
                if (!startFall)
                {
                    animator.SetTrigger("JumpTrigger");
                    startFall = true;
                }
            }
            if (canDoubleJump && doublejumping && inputJump && !doublejumped && isFalling)
            {
                //Apply the current movement to launch velocity.
                rb.velocity += doublejumpSpeed * Vector3.up;
                animator.SetInteger("Jumping", 3);
                doublejumped = true;
            }
        }
    }

    public IEnumerator _Jump()
    {
        isJumping = true;
        animator.SetInteger("Jumping", 1);
        animator.SetTrigger("JumpTrigger");
        //Apply the current movement to launch velocity.
        rb.velocity += jumpSpeed * Vector3.up;
        canJump = false;
        yield return new WaitForSeconds(.5f);
        isJumping = false;
    }

    /// <summary>
    /// Controls movement of character while in the air.
    /// </summary>
    void AirControl()
    {
        if (!isGrounded)
        {
            CameraRelativeMovement();
            Vector3 motion = inputVec;
            motion *= (Mathf.Abs(inputVec.x) == 1 && Mathf.Abs(inputVec.z) == 1) ? 0.7f : 1;
            rb.AddForce(motion * inAirSpeed, ForceMode.Acceleration);
            //Limit the amount of velocity character can achieve.
            float velocityX = 0;
            float velocityZ = 0;
            if (rb.velocity.x > maxVelocity)
            {
                velocityX = GetComponent<Rigidbody>().velocity.x - maxVelocity;
                if (velocityX < 0)
                {
                    velocityX = 0;
                }
                rb.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
            }
            if (rb.velocity.x < minVelocity)
            {
                velocityX = rb.velocity.x - minVelocity;
                if (velocityX > 0)
                {
                    velocityX = 0;
                }
                rb.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
            }
            if (rb.velocity.z > maxVelocity)
            {
                velocityZ = rb.velocity.z - maxVelocity;
                if (velocityZ < 0)
                {
                    velocityZ = 0;
                }
                rb.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
            }
            if (rb.velocity.z < minVelocity)
            {
                velocityZ = rb.velocity.z - minVelocity;
                if (velocityZ > 0)
                {
                    velocityZ = 0;
                }
                rb.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
            }
        }
    }

    #endregion

    #region Actions
    

   
    //public void GetHit()
    //{
    //    int hits = 5;
    //    int hitNumber = Random.Range(1, hits + 1);
    //    animator.SetInteger("Action", hitNumber);
    //    animator.SetTrigger("GetHitTrigger");
    //    StartCoroutine(_LockMovementAndAttack(.1f, .4f));
    //    //Apply directional knockback force.
    //    if (hitNumber <= 2)
    //    {
    //        StartCoroutine(_Knockback(-transform.forward, 8, 4));
    //    }
    //    else if (hitNumber == 3)
    //    {
    //        StartCoroutine(_Knockback(-transform.right, 8, 4));
    //    }
    //    else if (hitNumber == 4)
    //    {
    //        StartCoroutine(_Knockback(transform.forward, 8, 4));
    //    }
    //    else if (hitNumber == 5)
    //    {
    //        StartCoroutine(_Knockback(transform.right, 8, 4));
    //    }
    //}

    
    #endregion

    #region Misc

    public void Death()
    {
        animator.SetInteger("Action", 1);
        animator.SetTrigger("DeathTrigger");
        StartCoroutine(_LockMovementAndAttack(.1f, 1.5f));
        isDead = true;
        animator.SetBool("Moving", false);
        inputVec = new Vector3(0, 0, 0);
    }

    public void Revive()
    {
        animator.SetInteger("Action", 1);
        animator.SetTrigger("ReviveTrigger");
        isDead = false;
    }

    //Kkeep character from moveing while attacking, etc.
    public IEnumerator _LockMovementAndAttack(float delayTime, float lockTime)
    {
        yield return new WaitForSeconds(delayTime);
        canAction = false;
        canMove = false;
        animator.SetBool("Moving", false);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        inputVec = new Vector3(0, 0, 0);
        animator.applyRootMotion = true;
        yield return new WaitForSeconds(lockTime);
        canAction = true;
        canMove = true;
        animator.applyRootMotion = false;
    }

    //Placeholder functions for Animation events.
    public void Hit()
    {
    }

    public void Shoot()
    {
    }

    public void FootR()
    {
    }

    public void FootL()
    {
    }

    public void Land()
    {
    }

    public void Jump()
    {
    }

    #endregion

}
