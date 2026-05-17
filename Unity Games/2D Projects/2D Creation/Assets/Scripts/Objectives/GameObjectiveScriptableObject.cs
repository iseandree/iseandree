using UnityEngine;

[CreateAssetMenu(fileName = "New Objective")]
public class GameObjectiveScriptableObject : ScriptableObject
{
    public string objectiveName;
    public string objectiveDescription;
    public bool isCompleted;

    [Header("Rewards")]
    public int auraPoints; // I want to manipulate the color of the player's aura based on aura points obtained through quests
    // I might need to use a different value or at least use these values in the game manager to make the game manager add or remove value from the players aura color
}
