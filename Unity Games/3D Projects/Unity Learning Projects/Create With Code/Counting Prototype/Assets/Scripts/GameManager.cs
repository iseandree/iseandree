using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    // Serialize Field Variables
    [SerializeField] private GameObject[] pointBoxes;
    [SerializeField] private Text creditsText;
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject instructionScreen;
    [SerializeField] private GameObject continueScreen;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameBallSpawner gameBallSpawner;
    [SerializeField] private int credits = 0;
    [SerializeField] private int creditsToWin = 500;
    [SerializeField] private int creditsToLose = -250;
    [SerializeField] private int creditsToAlmostLose = -100;

    // Private Variables
    private FallingBall fallingBall;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize Variables
        gameBallSpawner = GameObject.Find("Game Ball Spawner").GetComponent<GameBallSpawner>();
        fallingBall = GameObject.Find("Game Ball").GetComponent<FallingBall>();
        pointBoxes = GameObject.FindGameObjectsWithTag("Point Box");
        credits = 20;
        creditsText.text = "Credits: " + credits;
    }

    // Update is called once per frame
    void Update()
    {
            if (Input.GetKeyDown(KeyCode.R) && !fallingBall.isDropped)
            {
                Reroll();
            }

            if(credits <= creditsToAlmostLose)
            {
                continueScreen.SetActive(true);
                Time.timeScale = 0;
            }

            if(credits <= creditsToLose)
            {
                endScreen.SetActive(true);
            }

            if(credits >= creditsToWin)
            {
                endScreen.SetActive(true);
            }
    }

    public void Reroll()
    {
        credits -= 5;
        creditsText.text = "Credits: " + credits;
        foreach (GameObject box in pointBoxes)
        {
            box.GetComponent<PointBox>().ResetValues();
        }
    }

    // Add the points earned from the ball landing in the point box (For Point Box Script)
    public void AddPoints(int points)
    {
        credits += points;
        creditsText.text = "Credits: " + credits;
        foreach(GameObject box in pointBoxes)
        {
            box.GetComponent<PointBox>().ResetValues();
        }
    }

    // Remove the Instructions and start the game
    public void StartGame()
    {
        instructionScreen.SetActive(false);
    }

    // Remove the Title and Show the Instructions
    public void ExplainGame()
    {
        titleScreen.SetActive(false);
        instructionScreen.SetActive(true);
    }

    // Remove the Continue Screen and continue gameplay
    public void ContinueGame()
    {
        continueScreen.SetActive(false);
        Time.timeScale = 1;
    }
    
    // Close the game after a short delay
    public void QuitGame()
    {
        StartCoroutine(EndGame(5));
    }

    // Start the end game delay process
    public IEnumerator EndGame(int delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        Application.Quit();
    }
}
