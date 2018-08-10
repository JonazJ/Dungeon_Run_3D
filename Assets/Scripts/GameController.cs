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
    public Text infoText;
    private bool gameEnded;

    //TimeText
    public Text timerText;
    private float secondsCount;
    private float minuteCount;

    //GameMenu && Paus
    private bool Paused = false;
    public GameObject messageWindow;
    public GameObject restartBtn;
    public GameObject backBtn;
    public Button startGameBtn;

    //Volume
    public Text volumeText;
    public Slider volumeSlider;

    void Start()
    {
        Paused = true;
        Time.timeScale = 0f;
        gameEnded = false;
        infoText.gameObject.SetActive(true);
        gateHigh.gameObject.SetActive(true);
        gateLow.gameObject.SetActive(true);
        startGameBtn.gameObject.SetActive(true);
        restartBtn.gameObject.SetActive(false);
        backBtn.gameObject.SetActive(false);
        infoText.text = "Objective: \nRetrieve all the coins without dying. \n \nHow to move: \nW - Forward \nS - Backward \nA - Left \nD - Right \n \nPress Esc to toggle Menu.";

        // Listener for Start game button
        startGameBtn.onClick.AddListener(TaskOnClick);
    }

    void Update()
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
            GameMenu(1);
        }
        
    }
    void TaskOnClick()
    {
        volumeSlider.gameObject.SetActive(false);
        GameMenu(0);
    }

    // Here is what will happen during certain steppes along the games progression.
    public void Result(bool alive)
    {
        if (alive == true)
        {
            // Winning action 
            infoText.text = "Gongratulations Dear Winner! \n" + "You managed to defeat the evil skeleton in level 1. \n" + "Time: " + minuteCount + ":" + (int)secondsCount + "\tScore: " + score;
        }
        else
        {
            // Defeat action
            infoText.text = "You died like an undead due to asphyxiation \n" + "Thank you for playing all aspects of our game! \n" + "Time: " + minuteCount + ":" + (int)secondsCount + "Score: " + score;
        }
        GameMenu(3);
    }

    //Method for handling Pausing
    public void GameMenu(int state)
    {
        switch (state)
        {
            case 1:
                restartBtn.gameObject.SetActive(true);
                backBtn.gameObject.SetActive(true);
                break;
            case 2:
                infoText.gameObject.SetActive(true);
                startGameBtn.gameObject.SetActive(true);
                break;
            case 3:
                infoText.gameObject.SetActive(true);
                restartBtn.gameObject.SetActive(true);
                backBtn.gameObject.SetActive(true);
                break;
            case 0:
                break;
        }
        if (Paused == true)
        {
            messageWindow.gameObject.SetActive(false);
            Cursor.visible = false;
            Paused = false;
            Time.timeScale = 1;
            startGameBtn.gameObject.SetActive(false);
            restartBtn.gameObject.SetActive(false);
            backBtn.gameObject.SetActive(false);
        }
        else
        {
            messageWindow.gameObject.SetActive(true);
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
        if (score == 5)
        {
            gateLow.gameObject.SetActive(false);
        }
        else if (score == 9)
        {
            gateHigh.gameObject.SetActive(false);
        }
        else if (score == 16)
        {
            Result(true);
        }
    }

}
