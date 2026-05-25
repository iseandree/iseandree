using System.Collections.Generic;
using TMPro;
using UnityEditor.Overlays;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour, IDataPersistence
{
    private Dictionary<ObjectiveSO, Dictionary<QuestObjective, int>> objectiveProgress = new Dictionary<ObjectiveSO, Dictionary<QuestObjective, int>>();

    public void UpdateObjectiveProgress(ObjectiveSO objectiveSO, QuestObjective questObjective)
    {
        if(!objectiveProgress.ContainsKey(objectiveSO))
        {
            objectiveProgress[objectiveSO] = new Dictionary<QuestObjective, int>();
        }

        var progressDictionary = objectiveProgress[objectiveSO];
        int newAmount = 0;

        if(questObjective.targetItem != null)
        {
            newAmount = InventoryManager.Instance.GetItemQuantity(questObjective.targetItem);
        }
        else if(questObjective.targetNPC != null && GameManager.Instance.dialogueHistoryTracker.HasSpokenWith(questObjective.targetNPC))
        {
            newAmount = questObjective.requiredAmount;
        }

        progressDictionary[questObjective] = newAmount;
    }
    public string GetProgressText(ObjectiveSO objectiveSO, QuestObjective questObjective)
    {
        int currentAmount = GetCurrentAmount(objectiveSO, questObjective);

        if(currentAmount >= questObjective.requiredAmount)
        {
            return "Complete";
        }
        else if(questObjective.targetItem != null)
        {
            return $"{currentAmount} / {questObjective.requiredAmount}";
        }
        else
        {
            return "In Progress";
        }

    }

    public int GetCurrentAmount(ObjectiveSO objectiveSO, QuestObjective questObjective)
    {
        if(objectiveProgress.TryGetValue(objectiveSO, out var objectiveDictionary))
        {
            if(objectiveDictionary.TryGetValue(questObjective, out int amount))
            {
                return amount;
            }
        }
        return 0;
    }

    public void SaveData(ref GameData gameData)
    {

    }
    public void LoadData(GameData gameData)
    {

    }
}
