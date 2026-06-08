using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

// Day/Night Cycle inspired/soruced by Mina Pêcheux from https://medium.com/codex/creating-a-basic-day-and-night-cycle-in-unity-c-dff942c1690d
public class GameManager : MonoBehaviour, IDataPersistence
{
    // Instance Variables
    public static GameManager Instance;
    public DialogueManager dialogueManager;
    public DialogueHistoryTracker dialogueHistoryTracker;
    public ObjectiveManager objectiveManager;

    // Difficulty Variables
    public enum DifficultySettings { Easy, Normal, Hard };
    [SerializeField] public DifficultySettings difficultySelected;
    private float difficultyModifier;

    // Day/Night Cycle Variables
    [SerializeField] DayAndNightTimeStamp[] timeStamps;
    [SerializeField] float cycleLength; // the total duration of the cycle (from morning to morning) in seconds
    [SerializeField] Light2D light2D;
    private float countDown = 180.0f;
    private float timeStampDifference;
    private float currentCycleTime; // the current time that has elapsed in this cycle
    private float currentTimeStamp; // the exact time in seconds for the current time stamp
    private float nextTimeStamp; // the exact time in seconds for the next time stamp
    private int currentTimeStampIndex;   // index for the current time stamp
    private int nextTimeStampIndex; // index for the upcoming time stamp
    [System.Serializable]
    public struct DayAndNightTimeStamp
    {
        public float timeRatio;
        public Color color;
        public float intensity;
    }

    // Helper variables
    private bool isGameRunning;
    private bool isPaused = false;
    private PlayerInput playerInput;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject savePrompt;

    // Temporary Setup
    [SerializeField] private GameObject background;
    [SerializeField] private TextMeshProUGUI timerText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isGameRunning = true;
        isPaused = false;
        CycleTimeStamps();
        playerInput = FindFirstObjectByType<PlayerController2D>().GetComponent<PlayerInput>();
    }

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Subscribe Difficulty Select to the OnDifficultySelected event that is activated from the main menu
    private void OnEnable()
    {
        GameEvents.OnDifficultySelected += DifficultySelect;
    }

    // Unsubscribe Difficulty Select to the OnDifficultySelected event that is activated from the main menu
    private void OnDisable()
    {
        GameEvents.OnDifficultySelected -= DifficultySelect;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isGameRunning)
        {
            return;
        }

        // Count the total time elapsed since the game has started and have it wrap back to 0 when the cycle ends using modulo
        currentCycleTime = (currentCycleTime + Time.deltaTime) % cycleLength;

        ApplyLightTransition();

        // Check if a time stamp has passed, compare the updated currentCycleTime with the time in seconds of the next time stamp
        if (currentCycleTime >= nextTimeStamp)
        {
            // If we aren't at the very end of the array, move to next stamp
            if (currentTimeStampIndex < timeStamps.Length - 1)
            {
                CycleTimeStamps();
            }
            // If we are at the end, wrap back to the start or stop the game
            else if (currentCycleTime >= cycleLength)
            {
                // To loop the day, reset currentCycleTime to 0 and call CycleTimeStamps
                // To stop the game, keep your current logic:
                isGameRunning = false;
            }
        }

        timerText.text = currentCycleTime.ToString();
    }

    // Blend color and intensity between timestamps
    private void ApplyLightTransition()
    {
        float lerpTime = (currentCycleTime - currentTimeStamp) / timeStampDifference;
        DayAndNightTimeStamp current = timeStamps[currentTimeStampIndex];
        DayAndNightTimeStamp next = timeStamps[nextTimeStampIndex];
        light2D.color = Color.Lerp(current.color, next.color, lerpTime);
        light2D.intensity = Mathf.Lerp(current.intensity, next.intensity, lerpTime);
    }

    // Advances the current and next time stamp indices to the next positions in the cycle and updates their
    // corresponding time values and the duration between them.
    private void CycleTimeStamps()
    {
        // Increment the currentTimeStampIndex, but with a modulo operation to ensure it doesn’t overshoot the length of the marks array
        currentTimeStampIndex = (currentTimeStampIndex + 1) % timeStamps.Length; // should be index 0 to start then increase
        nextTimeStampIndex = (currentTimeStampIndex + 1) % timeStamps.Length; // should be index 1 to start then increase

        /* By multiplying the normalised time ratio of the time stamp by the total length of the cycle the indices can
        get back to the actual DayAndNightTimeStamp objects from the array and store their actual time in the cycle in seconds */
        currentTimeStamp = timeStamps[currentTimeStampIndex].timeRatio * cycleLength;
        nextTimeStamp = timeStamps[nextTimeStampIndex].timeRatio * cycleLength;

        // The total duration between the current and next time stamps
        timeStampDifference = nextTimeStamp - currentTimeStamp;

        // Reached the last mark in the cycle re-add the cycle length to this duration to get back a positive number
        if (timeStampDifference < 0)
        {
            timeStampDifference += cycleLength;
        }
    }

    // Selects the game difficulty and updates related game state based on the specified difficulty level.
    public void DifficultySelect(int difficulty)
    {
        switch (difficulty)
        {
            case 0:
                difficultyModifier = 1.5f;
                difficultySelected = DifficultySettings.Easy;
                isGameRunning = true;
                break;
            case 1:
                difficultyModifier = 1.0f;
                difficultySelected = DifficultySettings.Normal;
                isGameRunning = true;
                break;
            case 2:
                difficultyModifier = 0.5f;
                difficultySelected = DifficultySettings.Hard;
                isGameRunning = true;
                break;
            default:
                break;
        }

        cycleLength = countDown * difficultyModifier;
    }

    // Using the player input, Pause the game and show the menu
    public void PauseGame()
    {
        isPaused = true;
        SwitchToUIMap();
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    // Using a button in game, resume the game 
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        SwitchToPlayerMap();
    }

    // Wait to Quit after a second or so, so that the game can save
    private IEnumerator QuitGameOnTimer()
    {
        yield return new WaitForSecondsRealtime(1);
        Application.Quit();
    }

    // Can't trigger QuitGameOnTimer on button so this is a buffer method really
    public void QuitGame()
    {
        StartCoroutine(QuitGameOnTimer());
    }

    // When the player is done using the UI system switch back to regular player input
    private void SwitchToPlayerMap()
    {
        if (playerInput != null)
        {
            // Changes the active map back to gameplay. Replace "Player" with your exact player map name.
            playerInput.SwitchCurrentActionMap("Player");

            // Optional: Re-lock cursor for gameplay
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // When the player interacts with NPCs or objects that provide dialogue switch to the UI map to navigate the UI
    private void SwitchToUIMap()
    {
        if (playerInput != null)
        {
            // Changes the active map to "UI". Replace "UI" with your exact UI map name.
            playerInput.SwitchCurrentActionMap("UI");

            // Optional: Unlock cursor for MnK players if your menu allows mouse clicking
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // Updates the specified <see cref="GameData"/> instance with the current game state values.
    public void SaveData(ref GameData data)
    {
        data.gameDifficulty = difficultySelected;
        data.currentTimeStampIndex = currentTimeStampIndex;
        data.currentCycleTime = currentCycleTime;
    }

    // Loads the specified game data into this current game state
    public void LoadData(GameData data)
    {
        this.difficultySelected = data.gameDifficulty;
        DifficultySelect((int)this.difficultySelected);
        this.currentTimeStampIndex = data.currentTimeStampIndex;
        this.currentCycleTime = data.currentCycleTime;
        Debug.Log("Difficulty Loaded: " + (int)difficultySelected +
            " Current Time Stamp Index: " + currentTimeStampIndex + " Current Time Elapsed: " + currentCycleTime);
    }
}
