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

        if (gameEnded == true)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Scene level = SceneManager.GetActiveScene();
                SceneManager.LoadScene(level.name);
            }
        }
    }

    public void EndGame()
    {
        gameEnded = true;
        if ( score == 5 )
        {
            endText.text = "Gratz, you've finished the game.";
            endText.gameObject.SetActive(true);
        }
        else if ( score == 6 )
        {
            endText.text = "So, you're doing everything...";
        }
        else if (score ==  8)
        {
            endText.text = "Stop playing already!";
        }
        else if (score == 16)
        {
            endText.text = "Okey game over, nothing more to see!";
        }
        else if (score == 16)
        {
            endText.text = "This time you actually finished the game. Impressive!";


        }
        else
        {

            endText.gameObject.SetActive(true);

        }




        
    }

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
