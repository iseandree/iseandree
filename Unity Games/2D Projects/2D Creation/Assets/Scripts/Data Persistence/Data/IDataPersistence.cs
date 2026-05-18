using UnityEngine;

/* The interface that oversees Saving and Loading.
 Code sourced/inspired by Shaped by Rain Studios https://www.youtube.com/watch?v=aUi9aijvpgs */
public interface IDataPersistence
{
    void LoadData(GameData data);

    void SaveData(ref GameData data);
}
