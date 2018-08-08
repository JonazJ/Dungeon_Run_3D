using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    //Gates
    public GameObject gateHigh;
    public GameObject gateLow;

    //ScoreText;
    public Text scoreText;
    private int score = 0;

    //EndText
    public Text endText;
    private bool gameEnded;

    //TimeText
    public Text timerText;
    private float secondsCount;
    private float minuteCount;

    //GameMenu && Paus
    private bool Paused = false;
    public GameObject Canvas;
    public GameObject TutorialWindow;

    void Start ()
    {
        Paused = true;
        Time.timeScale = 0f;
        gameEnded = false;
        endText.gameObject.SetActive(false);
        Canvas.gameObject.SetActive(false);
        gateHigh.gameObject.SetActive(true);
        gateLow.gameObject.SetActive(true);

    //Removing cursor during gameplay
    Cursor.visible = false;

    }
	void Update ()
    {
         
        //Set the timer in UI
        if (Paused == false)
        {
        secondsCount += Time.deltaTime;
        timerText.text = minuteCount + ":" + (int)secondsCount;
        }
        if (secondsCount >= 60)
        {
            minuteCount++;
            secondsCount = 0;
        }
        // Toggle On/Off the Manu during the game.
        // To Fix!! Add So game and inputs gets Paused!
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PausMenu();
        }
        
        while (gameEnded == true)
        {

        }
    }


    // Here is what will happen during certain steppes along the games progression.
    public void EndGame(bool alive)
    {
        if (alive == false)
        {
            // Defeat action
        }
        else
        {
            //Winning action
        }

        gameEnded = true;
    }
        
    //Method for handling Pausing
    public void PausMenu()
    {
        if (Paused == true)
        {
            TutorialWindow.gameObject.SetActive(false);
            Canvas.gameObject.SetActive(false);
            Cursor.visible = false;
            Paused = false;
            Time.timeScale = 1;
        }
        else
        {
            Canvas.gameObject.SetActive(true);
            Cursor.visible = true;
            Paused = true;
            Time.timeScale = 0f;
        }
    }

    // Method for updating score and pickups are gathered.
    public void AddScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;

        if (score == 1)
        {
            // Monster Spawning

        }
        else if (score == 10)
        {

            gateLow.gameObject.SetActive(false);

        }
        else if (score == 15)
        {
            gateHigh.gameObject.SetActive(false);
        }
        else if (score == 16)
        {
            //  endText.text = "You've won! Do you feel the positive emotions emerging!";
            EndGame(true);
        }

    }

    //public void TrapHandler()
    //{
    //    if (gateLow == true)
    //    {
    //        gateHigh.gameObject.SetActive(true);
    //        gateLow.gameObject.SetActive(false);
    //    }
    //    else
    //    {
    //        gateHigh.gameObject.SetActive(false);
    //        gateLow.gameObject.SetActive(true);
    //    }
    //}


}
