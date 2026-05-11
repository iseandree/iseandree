using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayOnEasy()
    {
        GameEvents.RaiseDifficultySelected(0);
        SceneManager.LoadScene("GameScene");
    }

    public void PlayOnNormal()
    {
        GameEvents.RaiseDifficultySelected(1);
        SceneManager.LoadScene("GameScene");
    }

    public void PlayOnHard()
    {
        GameEvents.RaiseDifficultySelected(2);
        SceneManager.LoadScene("GameScene");
    }
}
