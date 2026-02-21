using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    // Private Field
    [SerializeField] private List<GameObject> pointBoxes;
    [SerializeField] private Text creditsText;
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject instructionScreen;
    [SerializeField] private GameObject continueScreen;
    [SerializeField] private GameObject endScreen;
    private GameBallSpawner gameBallSpawner;
    public bool isGameRunning = false;
    public bool isContinuing = false;
    private int credits = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameBallSpawner = GameObject.Find("Game Ball Spawner").GetComponent<GameBallSpawner>();

        credits = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(isGameRunning)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if(credits <= -100)
            {
                continueScreen.SetActive(true);
            }
        }
    }


    public void AddPoints(int points)
    {
        credits += points;
        creditsText.text = "Credits : " + credits;
    }

    public void StartGame()
    {
        isGameRunning = true;
        gameBallSpawner.SpawnGameBall();

    }

    public void ExplainGame()
    {

    }

    public void ContinueGame()
    {

    }

    public void EndGame()
    {
        Application.Quit();
    }
}
