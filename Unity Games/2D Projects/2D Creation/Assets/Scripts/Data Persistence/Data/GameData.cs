using System.Collections.Generic;
using UnityEngine;
using static GameManager;

/* Represents the persistent state of the game, including player progress, collected items, checkpoints, and gameplay
  settings. Code inspired by and sourced by Night Run Studio https://www.youtube.com/playlist?list=PLSR2vNOypvs7sV_ks7h42F7hZ7DmGJqU6 */
[System.Serializable]
public class GameData 
{
    // Variables - Game Scene related
    public DifficultySettings gameDifficulty;
    public int currentTimeStampIndex;
    public float currentCycleTime;
    public Vector3 playerPosition;


    // Variables - Game Mechanic related
    public int auraPoints;
    public SerializableDictionary<string, bool> checkPointsCrossed;
    public SerializableDictionary<string, bool> itemsCollected;
    public InventorySlot slot;
    public Vector3 auraScale;

    
    public GameData()
    {
        this.gameDifficulty = DifficultySettings.Normal;
        this.currentTimeStampIndex = -1;
        this.currentCycleTime = 0;
        this.auraPoints = 0;
        playerPosition = Vector3.zero;
        checkPointsCrossed = new SerializableDictionary<string, bool>();
        itemsCollected = new SerializableDictionary<string, bool>();
        this.slot = new InventorySlot
        {
            itemSO = null,
            quantity = 0
        };
        this.auraScale = new Vector3(0, 0, 0);
    }

}
