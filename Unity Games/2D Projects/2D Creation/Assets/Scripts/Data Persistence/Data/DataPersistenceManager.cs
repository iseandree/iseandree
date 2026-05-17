using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName; // File we want to save data to

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            return;
        }
        instance = this;
    }


    private void Start()
    {
        // Give the OS standard directory for persisting data for a Unity Project
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        // Find all scripts that can implement IDataPersistence, must extend from Monobehaviour
        IEnumerable<IDataPersistence> dataPersistenceObjects = 
            FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects); // Return a new list and pass the result into list
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        // Load any saved data from a file using data handler
        this.gameData = dataHandler.Load();

        // If no data, initialize new game
        if(this.gameData == null)
        {
            NewGame();
        }

        // Push the loaded data to all other scripts that need it
        foreach(IDataPersistence dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        // Pass the data to other scripts so they can update it
        foreach(IDataPersistence dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.SaveData(ref gameData);
        }

        // Save that data to a dile using the data Handler
        dataHandler.Save(gameData);
    }
}
