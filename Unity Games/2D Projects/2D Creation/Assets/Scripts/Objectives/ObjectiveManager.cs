using System.Collections.Generic;
using UnityEngine;

// Manages the lifecycle, progress, and completion state of objectives within the game, including tracking active and
// completed objectives, updating progress, and handling rewards. - Need to save the objecitve progress dictionary and completed objectives and load them aswell
public class ObjectiveManager : MonoBehaviour, IDataPersistence
{
    // Private Variables
    private Dictionary<ObjectiveSO, Dictionary<QuestObjective, int>> objectiveProgress = new();
    private List<ObjectiveSO> completedObjectives = new();

    // Subscribe "IsObjectiveComplete" to Objective Events "IsObjectiveCompleted"
    private void OnEnable()
    {
        ObjectiveEvents.IsObjectiveCompleted += IsObjectiveComplete;
    }

    // Unsubscribe "IsObjectiveComplete" from Objective Events "IsObjectiveCompleted"
    private void OnDisable()
    {
        ObjectiveEvents.IsObjectiveCompleted -= IsObjectiveComplete;
    }

    // Determines whether all objectives defined in the specified objective set are complete based on current progress.
    public bool IsObjectiveComplete(ObjectiveSO objectiveSO)
    {
        // If objectiveProgress does not have the referenced objective in the dictionary return false
        if (!objectiveProgress.TryGetValue(objectiveSO, out var progressDict))
        {
            return false;
        }

        // Loop through the objectives in the referenced objective and update the progress of the objectives
        foreach (var objective in objectiveSO.objectives)
        {
            UpdateObjectiveProgress(objectiveSO, objective);
        }

        // Loop through the objectives in the referenced objective and if the objective in the progress dictionary does not meet
        // the required amount to be deemed complete return false
        foreach (var objective in objectiveSO.objectives)
        {
            if (progressDict[objective] < objective.requiredAmount)
            {
                return false;
            }    
        }

        return true;
    }

    // Check if completedObjectives list has the referenced objective and return true/false
    public bool GetCompletedObjectives(ObjectiveSO objectiveSO)
    {
        return completedObjectives.Contains(objectiveSO);
    }

    // Marks the specified objective as completed, removes its progress, and applies associated rewards and item
    // removals.
    public void CompleteObjective(ObjectiveSO objectiveSO)
    {
        if (objectiveSO == null) return;

        objectiveProgress.Remove(objectiveSO);
        completedObjectives.Add(objectiveSO);

        foreach (var objective in objectiveSO.objectives)
        {
            if(objective.targetItem != null && objective.requiredAmount > 0)
            {
                InventoryManager.Instance.RemoveItem(objective.targetItem, objective.requiredAmount);
            }
        }

        foreach (var reward in objectiveSO.rewards)
        {
            if (reward.auraScale > 0f)
            {
                InventoryManager.Instance.ChangeAuraScale(reward.auraScale);
            }

            if (reward.itemSO != null && reward.quantity > 0)
            {
                InventoryManager.Instance.AddItem(reward.itemSO, reward.quantity);
            }
        }
    }
    
    // Updates the progress of the specified quest objective based on the current game state.
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

    // Returns a user-friendly string representing the progress of the specified quest objective.
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

    // Gets the current progress amount for the specified quest objective within the given objective definition.
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

    // Return a list of the active objectives in the objectiveProgress dictionary
    public List<ObjectiveSO> GetActiveObjectives()
    {
        return new List<ObjectiveSO>(objectiveProgress.Keys);
    }

    // Check if the referenced objective is in the dictionary objectiveProgress, this would consider said objective
    // to be accepted
    public bool IsObjectiveAccepted(ObjectiveSO objectiveSO)
    {
        return objectiveProgress.ContainsKey(objectiveSO);
    }

    // Accept the referenced objective and store it in the objectiveProgress dictionary
    public void AcceptObjective(ObjectiveSO objectiveSO)
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
