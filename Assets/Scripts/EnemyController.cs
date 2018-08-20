using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    Transform player;               // Reference to the player's position.
    //PlayerHealth playerHealth;      // Reference to the player's health.
    //EnemyHealth enemyHealth;        // Reference to this enemy's health.
    NavMeshAgent nav;               // Reference to the nav mesh agent.


    private Animator animator;
    private Quaternion initRotation;

    public GameController controller;
    public PlayerController playerController;
    public GameObject playerObject;

    private int currentAnimation;
    private List<string> animations;

    private float speed;
    bool hit = false;



    public Weapon weapon;
    int rightWeapon = 0;
    int leftWeapon = 0;
    public bool canAction = true;
    public bool canMove = true;
    public int gameOverDelay;
    //Never used
    //Vector3 inputVec;


    Rigidbody rb;

    void Awake()
    {
        // Set up the references.
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //playerHealth = player.GetComponent<PlayerHealth>();
        //enemyHealth = GetComponent<EnemyHealth>();
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        speed = nav.speed;

        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        if (!hit)
        {

            // If the enemy and the player have health left...
            //if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0){
            // ... set the destination of the nav mesh agent to the player.
            nav.SetDestination(player.position);
            //}
            // Otherwise...
            //else{
            //    // ... disable the nav mesh agent.
            //    nav.enabled = false;
            //}

            //If the animator "SkeletonAC" is used---------------------------------------
            //animator.SetFloat("speedv", speed);
            //---------------------------------------------------------------------------

            //If the default RPG animator is used.---------------------------------------
            float velocityXel = transform.InverseTransformDirection(rb.velocity).x;
            float velocityZel = transform.InverseTransformDirection(rb.velocity).z;

            //Update animator with movement values
            animator.SetFloat("Velocity X", velocityXel / speed);
            animator.SetFloat("Velocity Z", velocityZel / speed);
            animator.SetBool("Moving", true);
            //---------------------------------------------------------------------------

        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Removes the player object, for testing.
            //other.gameObject.SetActive(false);
            //controller.AddScore(10);
            //controller.gateLow.gameObject.SetActive(false);
            //controller.EndGame(false);

            if (!hit)
            {

                Attack(2);
                playerController.Death();
                //Tries to end the game with a delay so that animations can finnish
                StartCoroutine(GameOver(gameOverDelay));

                //Ends the game
                //controller.Result(false);
                hit = true;
            }

        }

        else
        {
            //Debug.LogError("Unknown itemcollision");
        }
    }

    public void Attack(int attackSide)
    {
        if (canAction)
        {
            if (weapon == Weapon.UNARMED)
            {
                int maxAttacks = 3;
                int attackNumber = 0;
                if (attackSide == 1 || attackSide == 3)
                {
                    attackNumber = Random.Range(3, maxAttacks);
                }
                else if (attackSide == 2)
                {
                    attackNumber = Random.Range(6, maxAttacks + 3);
                }
                if (attackSide != 3)
                {
                    animator.SetInteger("Action", attackNumber);
                    if (leftWeapon == 12 || leftWeapon == 14 || rightWeapon == 13 || rightWeapon == 15)
                    {
                        StartCoroutine(_LockMovementAndAttack(0, .75f));
                    }
                    else
                    {
                        StartCoroutine(_LockMovementAndAttack(0, .6f));
                    }
                }
                else
                {
                    StartCoroutine(_LockMovementAndAttack(0, .75f));
                }
            }
            animator.SetTrigger("AttackTrigger");
        }
    }

    public IEnumerator _LockMovementAndAttack(float delayTime, float lockTime)
    {
        yield return new WaitForSeconds(delayTime);
        canAction = false;
        canMove = false;
        animator.SetBool("Moving", false);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        //Never used
        //inputVec = new Vector3(0, 0, 0);
        animator.applyRootMotion = true;
        yield return new WaitForSeconds(lockTime);
        canAction = true;
        canMove = true;
        animator.applyRootMotion = false;
    }
    IEnumerator GameOver(float time)
    {
        yield return new WaitForSeconds(time);
        // Code to execute after the delay
        controller.Result(false);
    }


    //Placeholder functions for Animation events.
    public void Hit()
    {
    }

    public void FootR()
    {
    }

    public void FootL()
    {
    }


}
