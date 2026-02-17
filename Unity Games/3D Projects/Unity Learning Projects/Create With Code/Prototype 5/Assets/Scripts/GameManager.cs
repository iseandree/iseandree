using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Private Variables
    [SerializeField] private List<GameObject> targets;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject volumeSlider;
    [SerializeField] private Button restartButton;
    [SerializeField] private GameObject titleScreen;
    private float spawnRate = 1.0f;
    private int score;
    private int lives;
    private bool canPause = false;
    private bool isPaused = false;

    // Public Variables
    public bool isGameActive;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P) && canPause && !isPaused)
        {
            PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.P) && canPause && isPaused)
        {
            UnPauseGame();
        }
    }

    public void UpdateLives()
    {
        livesText.text = "Lives: " + lives;
    }

    IEnumerator SpawnTarget()
    {
        while(isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targets.Count);
            Instantiate(targets[index]);
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    public void StartGame(int difficulty)
    {
        // Initialize Variables
        titleScreen.gameObject.SetActive(false);
        spawnRate /= difficulty;
        score = 0;
        lives = 3;
        livesText.text = "Lives: " + lives;
        isGameActive = true;
        canPause = true;
        volumeSlider.SetActive(false);

        // Call Methods
        StartCoroutine(SpawnTarget());
        UpdateScore(0);
    }

    public void LoseLife()
    {
        if(!isGameActive || lives < 0)
        {
            return;
        }

        lives--;
        UpdateLives();
        if(lives == 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        isGameActive = false;
        canPause = false;
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    } 
    
    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        isPaused = true;
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        isPaused = false;
    }
}
