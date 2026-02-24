using System.Drawing;
using System.IO;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Public Static Variables
    public static MenuManager Instance;

    // Public variables
    public string playerName;
    public int highScore;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadGame();
    }

    public void ReadStringInput(string name)
    {
        playerName = name;
        Debug.Log(playerName);
    }

    [System.Serializable]
    public class SaveData
    {
        public string playerName;
        public int highScore;
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData();
        saveData.playerName = playerName;
        saveData.highScore = highScore;

        string json = JsonUtility.ToJson(saveData);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            playerName = saveData.playerName;
            highScore = saveData.highScore;
        }
    }
}
