using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {


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

    //ToggleGameMenu
    private bool Paused = false;
    public GameObject Canvas;
    void Start ()
    {
        gameEnded = false;
        endText.gameObject.SetActive(false);
        Canvas.gameObject.SetActive(false);

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
            if(Paused == true)
            {
                Canvas.gameObject.SetActive(false);
                Cursor.visible = false;
                Paused = false;

            }
            else
            {
                Canvas.gameObject.SetActive(true);
                Cursor.visible = true;
                Paused = true;
            }
        }


        // To fix!!  Message after ended game with points, time and message before going back to menu.
        //if (gameEnded == true)
        //{
        //    if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        Scene level = SceneManager.GetActiveScene();
        //        SceneManager.LoadScene(level.name);
        //    }
        //}
    }


    // Here is what will happen during certain steppes along the games progression.
    public void EndGame()
    {

        if ( score == 5 )
        {
            endText.text = "Time to Advance to next level";
            endText.gameObject.SetActive(true);
        }
        else if (score ==  8)
        {
            endText.text = "Stop playing already!";
        }
        else if (score == 16)
        {
            gameEnded = true;
        }
        else
        {
            endText.gameObject.SetActive(true);

        }
        
    }

    // Method for updating score and pickups are gathered.
    public void AddScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;

        if (score >= 5)
        {
            EndGame();
        }
    }
}
