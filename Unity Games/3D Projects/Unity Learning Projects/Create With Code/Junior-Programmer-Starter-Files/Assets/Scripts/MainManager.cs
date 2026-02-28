using System.IO;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    // Public Static Variables
    public static MainManager Instance { get; private set; }

    // Public Variables
    public Color teamColor;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadColor();
    }

    [System.Serializable]
    private class SaveData
    {
        public Color teamColor;
    }

    public void SaveColor()
    {
        // Create a new instance of the save data and fill its team color class member with the teamColor variable saved in the MainManager
        SaveData saveData = new SaveData();
        saveData.teamColor = teamColor;

        // Transform into a Json
        string jSon = JsonUtility.ToJson(saveData);

        // Write a string to a File
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", jSon); // The first parameter is the path to the file, The second parameter is the text you want to write in that file
    }

    // This method is a reversal of the SaveColor method
    public void LoadColor()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        // Use the method File.Exists to check if a .json file exists
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);   // If the file does exist, then the method will read its content with File.ReadAllText
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);   // The resulting text to JsonUtility.FromJson to transform it back into a SaveData instance

            teamColor = saveData.teamColor; // Set the teamColor to the color saved in that SaveData
        }
    }
}
