using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour {


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

    // Update is called once per frame
    void Update ()
    {
        

		
	}
}
