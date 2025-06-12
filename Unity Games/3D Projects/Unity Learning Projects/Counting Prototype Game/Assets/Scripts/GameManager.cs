using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Restart game after pressing R
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("R key pressed");
            RestartGame();
        }   

    }

    /// <summary>
    /// Restart the game
    /// </summary>
    void RestartGame()
    {
        Debug.Log("Current Scene: " + SceneManager.GetActiveScene().name);
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
