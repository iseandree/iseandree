using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirectoryPath = "";  // The directory path of where to save data
    private string dataFileName = "";   // Name of the file to save to

    public FileDataHandler(string dataDirectoryPath, string dataFileName)
    {
        this.dataDirectoryPath = dataDirectoryPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        /* Use Path.Combine to account for different operating systems 
        having different path seperators */
        string fullPath = Path.Combine(dataDirectoryPath, dataFileName);

        GameData loadedData = null; // The variable to load into

        if(File.Exists(fullPath)) // Check if file actually exists
        {
            try
            {
                // Load the serialized data from the file
                string dataToLoad = "";
                using(FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // Deserialize the data from the file back into game data object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch(Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        /* Use Path.Combine to account for different operating systems 
        having different path seperators */
        string fullPath = Path.Combine(dataDirectoryPath, dataFileName);

        // If an error occurs when writing data let debugger know what happened
        try
        {
            // Create the directory the file will be written to if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // Serialize the game data object into Json
            string dataToStore = JsonUtility.ToJson(data, true);

            /* Write the serialized data to the file
            use using to ensure the connection to the file is closed when done reading or writing to file*/
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore); // Pass in data to write to file
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }
}
