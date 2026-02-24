using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIHandler : MonoBehaviour
{
    // Private Variables
    private MenuManager menuManager;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private GameObject errorText;
    [SerializeField] private Text bestText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuManager = GameObject.Find("Menu Manager").GetComponent<MenuManager>();
        errorText.SetActive(false);
        if(menuManager.playerName == null || menuManager.highScore == 0)
        {
            return;
        }
        bestText.text = "Best score: " + menuManager.playerName + " : " + menuManager.highScore;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadMainScene()
    {
        if (nameInput.text.Length == 0)
        {
            errorText.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("main");
        }
    }

    public void ExitGame()
    {
        if (Application.isEditor)
        {
            EditorApplication.ExitPlaymode();
        }
        else
        {
            Application.Quit();
        }
    }
}
