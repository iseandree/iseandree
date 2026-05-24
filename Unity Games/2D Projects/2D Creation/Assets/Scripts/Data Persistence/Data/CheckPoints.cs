using UnityEngine;

public class CheckPoints : MonoBehaviour, IDataPersistence
{
    // Generate a random id for items so we can keep track of which have been collected
    [SerializeField] private string iD;
    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        iD = System.Guid.NewGuid().ToString();
    }

    public bool isCrossed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            isCrossed = true;
            DataPersistenceManager.Instance.SaveGame();
        }
    }

    // Updates the specified <see cref="GameData"/> instance to reflect the current collection state of this item.
    public void SaveData(ref GameData data)
    {
        if (data.checkPointsCrossed.ContainsKey(iD))
        {
            data.checkPointsCrossed.Remove(iD);
        }

        data.checkPointsCrossed.Add(iD, isCrossed);
    }

    // Loads the collected item state from the specified game data and updates the object's active status accordingly.
    public void LoadData(GameData data)
    {
        data.checkPointsCrossed.TryGetValue(iD, out isCrossed);
        if (isCrossed)
        {
            gameObject.SetActive(false);
        }
    }
}
