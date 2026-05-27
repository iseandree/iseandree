using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Objective", menuName = "ObjectiveSO")]
public class ObjectiveSO : ScriptableObject
{
    public string objectiveName;
    [TextArea(3,5)] public string objectiveDescription;
    public string objectivePriority;
    public List<QuestObjective> objectives;

    [Header("Rewards")]
    public List<ObjectiveReward> rewards;
}

[System.Serializable]
public class QuestObjective
{
    public string description;
    [SerializeField] private Object target;
    public ItemSO targetItem => target as ItemSO;
    public ActorSO targetNPC => target as ActorSO;

    public int requiredAmount;
}

[System.Serializable]
public class ObjectiveReward
{
    // Can be rewarded in cooked meat or prepped veggies which would be used to deliver to the other clan.
    public ItemSO itemSO;
    public int quantity;

    //Can be rewared in aura to increase scale
    public float auraScale;

}