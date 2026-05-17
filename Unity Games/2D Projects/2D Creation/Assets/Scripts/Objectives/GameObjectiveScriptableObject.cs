using UnityEngine;

[CreateAssetMenu(fileName = "New Objective")]
public class GameObjectiveScriptableObject : ScriptableObject
{
    public string objectiveName;
    public string objectiveDescription;
    public bool isCompleted;
    public bool isAccepted;

    [Header("Rewards")]
    public float increaseAuraScale; 
}
