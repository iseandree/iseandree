using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSO", menuName = "Dialogue/DialogueNode")]
public class DialogueSO : ScriptableObject
{
    public DialogueLine[] lines;
    public DialogueOption[] options;

    [Header("Conditional Rquirements (Optional)")]
    public ActorSO[] requiredNPCs;
    public ItemScriptableObject[] requiredItems;

    [Header("Control Flags")]
    public bool removeAfterPlay;
    public List<DialogueSO> removeTheseOnPlay;

    public bool IsConditionMet()
    {
        // Check if the player has met the required NPC, if it is not the particular npc return false
        if(requiredNPCs.Length > 0)
        {
            foreach (var npc in requiredNPCs)
            {
                if(!GameManager.Instance.dialogueHistoryTracker.HasSpokenWith(npc))
                {
                    Debug.Log($"[Dialogue Condition] Failed NPC check for {npc.name}. Stopping evaluation.");
                    return false;
                }
            }
        }

        Debug.Log($"[Dialogue Condition] NPC requirements passed or empty. Total items to check: {requiredItems.Length}");

        // Check if the player has the required items in their inventory, if they don't have the particular item in their inventory return false
        if (requiredItems.Length > 0)
        {
            foreach(var item in requiredItems)
            {
                if(!InventoryManager.Instance.HasItem(item))
                {
                    Debug.Log("Checked if has:" + InventoryManager.Instance.HasItem(item) + " " + item);
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
    public ActorSO speaker;
    [TextArea(3, 5)] public string text;    // Sets the size of the box based on min and max of TextArea
}

[System.Serializable]
public class DialogueOption
{
    public DialogueSO nextDialogue;
    public string optionText;    
}