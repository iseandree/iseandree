using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSO", menuName = "Dialogue/DialogueNode")]
public class DialogueSO : ScriptableObject
{
    public DialogueLine[] lines;
    public DialogueOption[] options;

    [Header("Conditional Rquirements (Optional)")]
    public ActorSO[] requiredNPCs;
    //Items
    // Locations

    public bool IsConditionMet()
    {
        if(requiredNPCs.Length > 0)
        {
            foreach (var npc in requiredNPCs)
            {
                if(!DialogueHistoryTracker.Instance.HasSpokenWith(npc))
                {
                    return false;

                }
            }
            // checks fro items
            //checks fro locations
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