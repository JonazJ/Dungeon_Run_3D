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


    void Start ()
    {
        gameEnded = false;
        endText.gameObject.SetActive(false);

    }
	

	void Update ()
    {
        if (gameEnded)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Scene level = SceneManager.GetActiveScene();
                SceneManager.LoadScene(level.name);
            }
        }
    }

    public void EndGame(int status)
    {
        gameEnded = true;
        if ( status == 5 )
        {
            endText.text = "Gratz, you've finished the game.";
            endText.gameObject.SetActive(true);
        }
        else if ( status == 6 )
        {
            endText.text = "So, you're doing everything...";
        }
        else if (status ==  8)
        {
            endText.text = "Stop playing already!";
        }
        else if (status == 16)
        {
            endText.text = "Okey game over, nothing more to see!";
        }
        else if (status == 16)
        {
            endText.text = "This time you actually finished the game. Impressive! \nTo play again Press R.";

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
            EndGame(score);
        }
    }
}
