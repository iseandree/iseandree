using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

/* Represents a dialogue node containing dialogue lines, options, and associated objectives or requirements for use in
a dialogue system. Code sourced/inspired by Night Run Studios https://www.youtube.com/playlist?list=PLSR2vNOypvs6CNsu9fYk2v9DOZOTfSHPc */
[CreateAssetMenu(fileName = "DialogueSO", menuName = "Dialogue/DialogueNode")]
public class DialogueSO : ScriptableObject
{
    // Public variables
    public DialogueLine[] lines;
    public DialogueOption[] options;

    [Header("Control Flags")]
    public List<DialogueSO> removeTheseOnPlay;
    public bool removeAfterPlay;

    [Header("Completed Objective Requirements (Optional)")]
    public ObjectiveSO[] requiredCompletedObjectives;

    [Header("Conditional Rquirements (Optional)")]
    public ActorSO[] requiredNPCs;
    public ItemSO[] requiredItems;

    [Header("Objective Offer (Optional)")]
    public ObjectiveSO offerObjectiveOnEnd;

    [Header("Objective Turn-In (Optional)")]
    public ObjectiveSO turnInObjectiveOnEnd;


    // Determines whether all required conditions for the dialogue are met, including required NPC interactions,
    // inventory items, and completed objectives.
    public bool IsConditionMet()
    {
        // Check if the player has met the required NPC, if it is not the particular npc return false
        if(requiredNPCs.Length > 0)
        {
            foreach (var npc in requiredNPCs)
            {
                if(!GameManager.Instance.dialogueHistoryTracker.HasSpokenWith(npc))
                {
                    return false;
                }
            }
        }

        // Check if the player has the required items in their inventory, if they don't have the particular item in their inventory return false
        if (requiredItems.Length > 0)
        {
            foreach(var item in requiredItems)
            {
                if(!InventoryManager.Instance.HasItem(item))
                {
                    return false;
                }
            }
        }

        // Check if the player has completed a specific objective, if it is not completed return false
        if(requiredCompletedObjectives != null && requiredCompletedObjectives.Length > 0)
        {
            foreach (var objective in requiredCompletedObjectives)
            {
                if (!GameManager.Instance.objectiveManager.IsObjectiveComplete(objective))
                {
                    return false;
                }
            }
        }

        return true;
    }
}

[System.Serializable]
public class DialogueLine
{
    public ActorSO speaker; // Identifies which character is speaking
    [TextArea(3, 5)] public string text;    // Sets the size of the box based on min and max of TextArea
}

[System.Serializable]
public class DialogueOption
{
    public DialogueSO nextDialogue;
    public string optionText;
    public ObjectiveSO offerObjective;
}