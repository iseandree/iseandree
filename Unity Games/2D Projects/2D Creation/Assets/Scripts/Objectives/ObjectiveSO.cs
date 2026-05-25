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
    public float increaseAuraScale;
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