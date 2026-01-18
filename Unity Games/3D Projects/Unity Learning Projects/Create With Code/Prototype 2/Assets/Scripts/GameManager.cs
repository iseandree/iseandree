using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Private Variables
    private static int score;
    private static int lives;


    private void Awake()
    {
        score = 0;
        lives = 3;
        Debug.Log("Lives: " + lives);
        Debug.Log("Score: " + score);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(lives == 0)
        {
            Application.Quit();
            Debug.Log("Game Over");
        }
    }

    public static void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);
    }

    public static void LoseLife(int amount)
    {
        lives -= amount;
        Debug.Log("Lives: " + lives);
    }
}
