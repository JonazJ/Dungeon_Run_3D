using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float speed;
    private int score;
    //public UICharInfo scoreText;
    public Text scoreText;

    // Use this for initialization
    void Start()
    {
        scoreText = GetComponent<Text>();
        score = 0;
        //Ger null reference då text inte fungerar som det ska
        //scoreText.text = "Score " + score;
    }

    // Update is called once per frame
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
        if (other.gameObject.tag == "pickup")
        {
            other.gameObject.SetActive(false);
            //Ger null reference då text inte fungerar som det ska
            //score++;
            //scoreText.text = "Score " + score;

        }
    }

}
