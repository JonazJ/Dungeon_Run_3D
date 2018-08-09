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


    private GameController controller;
    private Animator animator;
    private Quaternion initRotation;

    public PlayerController playerController;
    public GameObject playerObject;

    private int currentAnimation;
    private List<string> animations;

    private float speed;


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

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Removes the player object, for debugging.
            //other.gameObject.SetActive(false);
            //controller.AddScore(10);
            //controller.gateLow.gameObject.SetActive(false);
            //controller.EndGame(false);

            playerController.Death();
            
        }

        else
        {
            Debug.LogError("Unknown itemcollision");
        }
    }

}