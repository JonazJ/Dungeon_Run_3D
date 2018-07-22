using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float speed;
    private GameController controller;


    void Start()
    {
        GameObject tmp = GameObject.FindGameObjectWithTag("GameController");
        controller = tmp.GetComponent<GameController>();
        if (controller == null)
        {
            Debug.LogError("Unable to find the GameController script");
        }
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        float moveH = Input.GetAxis("Horizontal");
        float moveV = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveH, 0.0f, moveV);

        Rigidbody body = GetComponent<Rigidbody>();
        body.AddForce(movement * speed * Time.deltaTime);
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
            Debug.LogError("Unknown itemcollision");
        }
        
    }
}
