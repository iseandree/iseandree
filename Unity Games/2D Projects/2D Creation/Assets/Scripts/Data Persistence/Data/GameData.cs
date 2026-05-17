using System.Collections.Generic;
using UnityEngine;
using static GameManager;

[System.Serializable]
public class GameData 
{
    public DifficultySettings gameDifficulty;
    public int currentTimeStampIndex;
    public int auraPoints;
    public float currentCycleTime;
    public Vector3 playerPosition;
    public Dictionary<string, bool> checkPointsCrossed;
    public Dictionary<string, bool> itemsCollected;
    public InventorySlot slot;

    public GameData()
    {
        this.gameDifficulty = DifficultySettings.Normal;
        this.currentTimeStampIndex = -1;
        this.currentCycleTime = 0;
        this.auraPoints = 0;
        playerPosition = Vector3.zero;
        checkPointsCrossed = new Dictionary<string, bool>();
        itemsCollected = new Dictionary<string, bool>();
        this.slot.itemSO = null;
        this.slot.quantity = 0;
    }

}
