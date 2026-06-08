using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Represents the main menu controller for the game, providing methods to start the game at different difficulty levels
// and to handle data persistence operations.
public class MainMenu : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button continueButton;

    // When game starts check if a save file exists on the player's end, if not don't show the continue button
    private void Start()
    {
        if (DataPersistenceManager.Instance != null && !DataPersistenceManager.Instance.HasGameData())
        {
            continueButton.gameObject.SetActive(false);
        }
        else if (DataPersistenceManager.Instance.HasGameData())
        {
            continueButton.gameObject.SetActive(true);
        }
    }

    // Handles the continuation to the next scene with data already loaded, pushing it to the Game Manager
    public void OnContinueGameClicked()
    {
        DataPersistenceManager.Instance.LoadGame();
        SceneManager.LoadScene("GameScene");
    }

    // Start the game on easy and send the difficulty selection to the Game Manager
    public void PlayOnEasy()
    {
        GameEvents.RaiseDifficultySelected(0);
        DataPersistenceManager.Instance.CreateAndStartNewGame();
        SceneManager.LoadScene("GameScene");
        Debug.Log(GameEvents.SavedDifficulty);
    }

    // Start the game on normal and send the difficulty selection to the Game Manager
    public void PlayOnNormal()
    {
        GameEvents.RaiseDifficultySelected(1);
        DataPersistenceManager.Instance.CreateAndStartNewGame();
        SceneManager.LoadScene("GameScene");
        Debug.Log(GameEvents.SavedDifficulty);

    }

    // Start the game on hard and send the difficulty selection to the Game Manager
    public void PlayOnHard()
    {
        GameEvents.RaiseDifficultySelected(2);
        DataPersistenceManager.Instance.CreateAndStartNewGame();
        SceneManager.LoadScene("GameScene");
        Debug.Log(GameEvents.SavedDifficulty);

    }
}
