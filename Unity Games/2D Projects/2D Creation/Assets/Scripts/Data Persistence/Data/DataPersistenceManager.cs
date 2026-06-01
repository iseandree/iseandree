using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Manages the persistence of game data by coordinating loading and saving operations across all registered data
persistence objects in the scene. Code sourced/inspired by Shaped by Rain Studios https://www.youtube.com/watch?v=aUi9aijvpgs */
public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager Instance { get; private set; }

    // Private Variables - Gets/Sets the name of the file used to store data.
    [Header("File Storage Config")]
    [SerializeField] private string fileName; // File to save data to
    [SerializeField] private bool useEncryption; // Encrypt or not
    
    // Private Variables - General
    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    // Initializes the singleton instance of the component if it has not already been set.
    private void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Subscribe to OnSceneLoaded
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Unsubscribe from OnSceneLoaded
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Initializes the data persistence system and loads the game state.
    private void Start()
    {
        // Give the OS standard directory for persisting data for a Unity Project
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    // Finds all active components in the scene that implement the IDataPersistence interface and are derived from MonoBehaviour.
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        // Find all scripts that can implement IDataPersistence, must extend from Monobehaviour
        IEnumerable<IDataPersistence> dataPersistenceObjects =
            FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects); // Return a new list and pass the result into list
    }

    // Initializes a new game by resetting all game data to their default values.
    public void NewGame()
    {
        this.gameData = new GameData();
    }

    /* Loads the game state from persistent storage and updates all registered data persistence objects with the loaded
     data. If no saved data is found, initializes a new game state. */
    public void LoadGame()
    {
        // Load any saved data from a file using data handler
        this.gameData = dataHandler.Load();

        // If no data, initialize new game
        if (this.gameData == null)
        {
            NewGame();
        }

        // Push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.LoadData(gameData);
        }
    }

    // Saves the current game state by persisting all relevant data using registered data persistence objects and the
    public void SaveGame()
    {
        // Pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.SaveData(ref gameData);
        }

        // Save that data to a file using the data Handler
        dataHandler.Save(gameData);
    }

    // When a scene is loaded find all the data persistence objects and push relevant data to them
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        PushDataToObjects();
    }

    // If there is a save file push the stored data to all objects that had their data stored
    private void PushDataToObjects()
    {
        if (this.gameData == null) return;

        foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.LoadData(gameData);
        }
    }

    // Return whether or not there is a save file
    public bool HasGameData()
    {
        return this.gameData != null;
    }
}
