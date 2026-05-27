using System.Collections.Generic;
using TMPro;
using UnityEditor.Overlays;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour, IDataPersistence
{
    private Dictionary<ObjectiveSO, Dictionary<QuestObjective, int>> objectiveProgress = new();
    private List<ObjectiveSO> completedObjectives = new();
    private void OnEnable()
    {
        ObjectiveEvents.IsObjectiveCompleted += IsObjectiveComplete;
    }
    private void OnDisable()
    {
        ObjectiveEvents.IsObjectiveCompleted -= IsObjectiveComplete;

    }

    public bool IsObjectiveComplete(ObjectiveSO objectiveSO)
    {
        if(!objectiveProgress.TryGetValue(objectiveSO, out var progressDict))
        {
            return false;
        }

        foreach (var objective in objectiveSO.objectives)
        {
            UpdateObjectiveProgress(objectiveSO, objective);
        }

        foreach (var objective in objectiveSO.objectives)
        {
            if (progressDict[objective] < objective.requiredAmount)
            {
                return false;
            }    
        }

        return true;
    }

    public void CompleteObjective(ObjectiveSO objectiveSO)
    {
        if (objectiveSO == null) return;

        objectiveProgress.Remove(objectiveSO);
        completedObjectives.Add(objectiveSO);
        foreach (var reward in objectiveSO.rewards)
        {
            if (reward.auraScale > 0f)
            {
                Debug.Log($"[ObjectiveManager] Awarding {reward.auraScale} Aura Scale!");

                // Fire the global event that your PlayerController2D is listening to!
                InventoryManager.Instance.ChangeAuraScale(reward.auraScale);
            }

            if (reward.itemSO != null && reward.quantity > 0)
            {
                Debug.Log($"[ObjectiveManager] Awarding Item: {reward.itemSO.name} x{reward.quantity}");
                InventoryManager.Instance.AddItem(reward.itemSO, reward.quantity);
            }
        }

    }

    public bool GetCompletedObjectives(ObjectiveSO objectiveSO)
    {
        return completedObjectives.Contains(objectiveSO);
    }
    public bool IsObjectiveAccepted(ObjectiveSO objectiveSO)
    {
        return objectiveProgress.ContainsKey(objectiveSO);
    }

    public List<ObjectiveSO> GetActiveObjectives()
    {
        return new List<ObjectiveSO>(objectiveProgress.Keys);
    }

    public void UpdateObjectiveProgress(ObjectiveSO objectiveSO, QuestObjective questObjective)
    {
        if(!objectiveProgress.ContainsKey(objectiveSO))
        {
            return;
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

    public void AcceptObjective(ObjectiveSO objectiveSO)
    {
        objectiveProgress[objectiveSO] = new Dictionary<QuestObjective, int>();

        foreach (var objective in objectiveSO.objectives)
        {
            UpdateObjectiveProgress(objectiveSO, objective);
        }
    }

    public void DeclineObjective(ObjectiveSO objectiveSO)
    {
        objectiveProgress[objectiveSO] = new Dictionary<QuestObjective, int>();
        foreach (var objective in objectiveSO.objectives)
        {
            UpdateObjectiveProgress(objectiveSO, objective);
        }
    }

    public void SaveData(ref GameData gameData)
    {

    }

    public void LoadData(GameData gameData)
    {

    }
}
